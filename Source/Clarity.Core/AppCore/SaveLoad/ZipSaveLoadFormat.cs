using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Clarity.Common.Infra.Files;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Core.AppCore.SaveLoad.Converters;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class ZipSaveLoadFormat : ISaveLoadFormat
    {
        public string Name => "Zip";
        public string FileExtension => ".cw";
        
        private readonly ISaveLoadConverterContainer converterContainer;
        private readonly ITrwFactory trwFactory;
        private readonly ISaveLoadFactory saveLoadFactory;

        public ZipSaveLoadFormat(ITrwFactory trwFactory, ISaveLoadFactory saveLoadFactory, ISaveLoadConverterContainer converterContainer)
        {
            this.converterContainer = converterContainer;
            this.trwFactory = trwFactory;
            this.saveLoadFactory = saveLoadFactory;
        }

        public void Save(IFileSaveInfo saveInfo)
        {
            using (var archive = new ZipArchive(saveInfo.FileSystem.OpenWriteNew(saveInfo.FilePath), ZipArchiveMode.Create, false, Encoding.UTF8))
            {
                var metaEntry = archive.CreateEntry("meta.json");
                using (var writer = trwFactory.JsonWriter(metaEntry.Open()))
                using (var context = saveLoadFactory.MetaWriteContext(writer))
                    context.Write(new SaveLoadMetadata { Version = converterContainer.CurrentVersion });

                IReadOnlyDictionary<Type, string> typeAliases;
                IDictionary<string, IAsset> assets = new Dictionary<string, IAsset>();
                var worldEntry = archive.CreateEntry("world.json");
                using (var writer = trwFactory.JsonWriter(worldEntry.Open()))
                using (var context = saveLoadFactory.WorldWriteContext(writer))
                {
                    context.Bag.Add(SaveLoadConstants.AssetDictBagKey, assets);
                    context.Write(saveInfo.World);
                    typeAliases = context.TypeAliases;
                }
                
                var aliasesEntry = archive.CreateEntry("typeAliases.json");
                using (var writer = trwFactory.JsonWriter(aliasesEntry.Open()))
                using (var context = saveLoadFactory.AliasesWriteContext(writer))
                {
                    var rawAliases = typeAliases.ToDictionary(x => x.Value, x => x.Key.AssemblyQualifiedName);
                    context.Write(rawAliases);
                }

                var assetInfos = assets.Values.Select(x =>
                {
                    var localCopyPath = x.StorageType ==  AssetStorageType.CopyLocal 
                        ? Path.Combine("Assets", x.Name, x.AssetCachePath)
                        : null;
                    return new AssetSaveLoadInfo(x, localCopyPath);
                }).ToArray();

                var assetInfoEntry = archive.CreateEntry("assetInfos.json");
                using (var writer = trwFactory.JsonWriter(assetInfoEntry.Open()))
                using (var context = saveLoadFactory.AssetsInfoWriteContext(writer))
                    context.Write(assetInfos);

                foreach (var asset in assets.Values)
                foreach (var localFilePath in saveInfo.GetAssetLocalFiles(asset))
                {
                    var fileEntry = archive.CreateEntry(Path.Combine("Assets", asset.Name, localFilePath));
                    using (var writeStream = fileEntry.Open())
                    using (var readStream = saveInfo.ReadAssetFile(asset, localFilePath))
                        readStream.CopyTo(writeStream);
                }
            }
        }

        public void Load(IFileLoadInfo loadInfo)
        {
            using (var archive = new ZipArchive(loadInfo.FileSystem.OpenRead(loadInfo.FilePath), ZipArchiveMode.Read, false, Encoding.UTF8))
            {
                SaveLoadMetadata metadata;
                var metaEntry = archive.GetEntry("meta.json") ?? throw new Exception("meta.json not found.");
                using (var reader = trwFactory.JsonReader(metaEntry.Open()))
                using (var context = saveLoadFactory.MetaReadContext(reader))
                    metadata = context.Read<SaveLoadMetadata>();

                AssetSaveLoadInfo[] assetInfos;
                var assetInfoEntry = archive.GetEntry("assetInfos.json") ?? throw new Exception("assetInfo.json not found.");
                using (var reader = trwFactory.JsonReader(assetInfoEntry.Open()))
                using (var convertedReader = converterContainer.ConvertAssetInfoReader(reader, metadata.Version, converterContainer.CurrentVersion))
                using (var context = saveLoadFactory.AssetsInfoReadContext(convertedReader))
                    assetInfos = context.Read<AssetSaveLoadInfo[]>();

                var assets = new Dictionary<string, IAsset>();
                var zipFileSystem = new ZipFileSystem(archive);
                var actualFileSystem = new ActualFileSystem();
                foreach (var assetInfo in assetInfos)
                {
                    var assetLoadInfo = new AssetLoadInfo
                    {
                        AssetName = assetInfo.AssetName,
                        StorageType = assetInfo.StorageType,
                        ReferencePath = assetInfo.ReferencePath
                    };
                    switch (assetInfo.StorageType)
                    {
                        case AssetStorageType.CopyLocal:
                            assetLoadInfo.FileSystem = zipFileSystem;
                            assetLoadInfo.LoadPath = assetInfo.LocalCopyPath;
                            break;
                        case AssetStorageType.ReferenceOriginal:
                            assetLoadInfo.FileSystem = actualFileSystem;
                            assetLoadInfo.LoadPath = assetInfo.ReferencePath;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    var asset = loadInfo.OnFoundAsset(assetLoadInfo);
                    assets.Add(asset.Name, asset);
                }

                IReadOnlyDictionary<string, Type> aliases;
                var aliasesEntry = archive.GetEntry("typeAliases.json") ?? throw new Exception("typeAliases.json not found.");
                using (var reader = trwFactory.JsonReader(aliasesEntry.Open()))
                using (var convertedReader = converterContainer.ConvertAliasesReader(reader, metadata.Version, converterContainer.CurrentVersion))
                using (var context = saveLoadFactory.AliasesReadContext(convertedReader))
                {
                    var rawAliases = context.Read<Dictionary<string, string>>();
                    aliases = rawAliases.ToDictionary(x => x.Key, x => Type.GetType(x.Value));
                }

                IWorld world;
                var worldEntry = archive.GetEntry("world.json") ?? throw new Exception("world.json not found.");
                using (var reader = trwFactory.JsonReader(worldEntry.Open()))
                using (var convertedReader = converterContainer.ConvertWorldReader(reader, metadata.Version, converterContainer.CurrentVersion))
                using (var context = saveLoadFactory.WorldReadContext(convertedReader, aliases))
                {
                    context.Bag.Add(SaveLoadConstants.AssetDictBagKey, assets);
                    world = context.Read<IWorld>();
                }
                loadInfo.OnLoadedWorld(world);
            }
        }
    }
}