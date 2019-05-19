using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Clarity.Common;
using Clarity.Common.CodingUtilities;
using Clarity.Common.GraphicalGeometry;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;
using Newtonsoft.Json.Linq;

namespace Clarity.Core.AppFeatures.Models
{
    public class CgModelLoader : IAssetLoader
    {
        private static readonly string[] SupportedExtensions = { ".cgm" };

        public string Name => "CORE.CGM";
        public string AssetTypeString => $"3D Model files (FlexibleModel) (${Name})";
        public IReadOnlyList<string> FileExtensions => SupportedExtensions;
        public AssetLoaderFlags Flags => AssetLoaderFlags.None;

        public bool LikesName(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return SupportedExtensions.Contains(extension);
        }

        public AssetLoadResultByLoader Load(AssetLoadInfo loadInfo)
        {
            try
            {
                var asset = BuildAsset(loadInfo);
                return AssetLoadResultByLoader.Success(asset);
            }
            catch (Exception ex)
            {
                return AssetLoadResultByLoader.Failure("EXCEPTION", ex);
            }
        }

        private static unsafe Asset BuildAsset(AssetLoadInfo loadInfo)
        {
            var fileData = loadInfo.FileSystem.ReadAllBytes(loadInfo.LoadPath);
            var pack = new ResourcePack(ResourceVolatility.Immutable);

            dynamic d;
            using (var reader = new StreamReader(new MemoryStream(fileData)))
                d = JObject.Parse(reader.ReadToEnd());

            var vertexSets = new List<IFlexibleModelVertexSet>();
            var vertexSetDisambiguator = 0;
            foreach (var dVertexSet in d.VertexSets)
            {
                var arrays = new List<RawDataResSubrange>();
                var arrayDisambiguator = 0;
                foreach (var dArraySubrange in dVertexSet.ArraySubranges)
                {
                    var data = (byte[])dArraySubrange.Data;
                    fixed (byte* pData = data)
                    {
                        var rawDataResource = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pData, data.Length);
                        arrays.Add(new RawDataResSubrange(rawDataResource, 0));
                        pack.AddSubresource("Array" + arrayDisambiguator++, rawDataResource);
                    }
                }

                var dIndicesInfo = dVertexSet.IndicesInfo;
                VertexIndicesInfo indicesInfo;
                if (dIndicesInfo == null)
                    indicesInfo = null;
                else
                {
                    var arrayIndex = (int)dIndicesInfo.ArrayIndex;
                    var format = CodingHelper.EnumParse<CommonFormat>((string)dIndicesInfo.Format);
                    indicesInfo = new VertexIndicesInfo(arrayIndex, format);
                }

                var elementInfos = new List<VertexElementInfo>();
                foreach (var dElementInfo in dVertexSet.ElementInfos)
                {
                    var arrayIndex = (int)dElementInfo.ArrayIndex;
                    var format = CodingHelper.EnumParse< CommonFormat>((string)dElementInfo.Format);
                    var stride = (int)dElementInfo.Stride;
                    var offset = (int)dElementInfo.Offset;
                    var commonSemantic = CodingHelper.EnumParse<CommonVertexSemantic>((string)dElementInfo.CommonSemantic);
                    var semantic = (string)dElementInfo.Semantic;
                    elementInfos.Add(commonSemantic != CommonVertexSemantic.Undefined
                        ? new VertexElementInfo(commonSemantic, arrayIndex, format, offset, stride)
                        : new VertexElementInfo(semantic, arrayIndex, format, offset, stride));
                }

                var vertexSet = new FlexibleModelVertexSet(ResourceVolatility.Immutable, arrays, elementInfos, indicesInfo);
                pack.AddSubresource("VertexSet" + vertexSetDisambiguator++, vertexSet);
                vertexSets.Add(vertexSet);
            }

            var parts = new List<FlexibleModelPart>();
            foreach (var dPart in d.Parts)
            {
                var vertexSetIndex = (int)dPart.VertexSetIndex;
                var primitiveTopology = CodingHelper.EnumParse<FlexibleModelPrimitiveTopology>((string)dPart.PrimitiveTopology);
                var vertexOffset = (int)dPart.VertexOffset;
                var firstIndex = (int)dPart.FirstIndex;
                var indexCount = (int)dPart.IndexCount;
                var modelMaterialName = (string)dPart.ModelMaterialName;
                parts.Add(new FlexibleModelPart
                {
                    VertexSetIndex = vertexSetIndex,
                    PrimitiveTopology = primitiveTopology,
                    VertexOffset = vertexOffset,
                    FirstIndex = firstIndex,
                    IndexCount = indexCount,
                    ModelMaterialName = modelMaterialName
                });
            }

            var radius = (float)(double)d.Radius;
            var model = new FlexibleModel(ResourceVolatility.Immutable, vertexSets, parts, radius);
            pack.AddSubresource("Model", model);
            pack.MainSubresource = model;

            var hash = AssetHashMd5.FromSingleFile(fileData);
            var fileName = Path.GetFileName(loadInfo.LoadPath);
            var asset = new Asset(loadInfo.AssetName, pack, AssetStorageType.CopyLocal, hash, loadInfo.ReferencePath, fileName);
            return asset;
        }
    }
}