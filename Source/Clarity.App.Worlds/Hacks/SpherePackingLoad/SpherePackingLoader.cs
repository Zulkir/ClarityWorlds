using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Clarity.App.Worlds.Assets;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Worlds.Hacks.SpherePackingLoad
{
    public class SpherePackingLoader : IAssetLoader
    {
        private static readonly string[] SupportedExtensions = { ".spk" };
        public string Name => "CORE.SpherePack";
        public string AssetTypeString => "Sphere Packings";
        public IReadOnlyList<string> FileExtensions => SupportedExtensions;
        public AssetLoaderFlags Flags => AssetLoaderFlags.None;

        public bool LikesName(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return SupportedExtensions.Contains(extension);
        }

        private static readonly Regex PointRegex = new Regex(@"^	\[CTLPT E7  (\S+) (\S+) (\S+) (\S+) (\S+) (\S+) (\S+)\]$");

        public AssetLoadResultByLoader Load(AssetLoadInfo loadInfo)
        {
            try
            {
                var points = new List<Vector3>();
                var loadPath = loadInfo.LoadPath;
                var fileData = loadInfo.FileSystem.ReadAllBytes(loadPath);
                using (var reader = new StreamReader(new MemoryStream(fileData)))
                    while (true)
                    {
                        var line = reader.ReadLine();
                        if (line == null)
                            break;
                        var match = PointRegex.Match(line);
                        if (!match.Success)
                            continue;
                        points.Add(new Vector3(
                            float.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture),
                            float.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture),
                            float.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture)
                            ));
                    }
                var resource = new SpherePackingResult(points);
                var hash = AssetHashMd5.FromSingleFile(fileData);
                var asset = new Asset(loadInfo.AssetName, resource, loadInfo.StorageType, hash, loadInfo.ReferencePath, Path.GetFileName(loadInfo.ReferencePath));
                return AssetLoadResultByLoader.Success(asset);
            }
            catch (Exception ex)
            {
                return AssetLoadResultByLoader.Failure("EXCEPTION", ex);
            }
        }
    }
}