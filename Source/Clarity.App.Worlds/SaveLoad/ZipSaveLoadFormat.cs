using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.SaveLoad.Converters;
using Clarity.Common.Infra.Files;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.SaveLoad;

namespace Clarity.App.Worlds.SaveLoad
{
    public class ZipSaveLoadFormat : ISaveLoadFormat
    {
        public string Name => "Zip";
        public string FileExtension => ".cw";
        
        private readonly ISaveLoadConverterContainer converterContainer;
        private readonly ITrwFactory trwFactory;
        private readonly ISaveLoadFactory saveLoadFactory;
        private readonly IResourceSavingService resourceSavingService;
        private readonly IResourceLoadingService resourceLoadingService;

        public ZipSaveLoadFormat(ITrwFactory trwFactory, ISaveLoadFactory saveLoadFactory, ISaveLoadConverterContainer converterContainer, 
            IResourceSavingService resourceSavingService, IResourceLoadingService resourceLoadingService)
        {
            this.converterContainer = converterContainer;
            this.resourceSavingService = resourceSavingService;
            this.resourceLoadingService = resourceLoadingService;
            this.trwFactory = trwFactory;
            this.saveLoadFactory = saveLoadFactory;
        }

        public void Save(IFileSaveInfo saveInfo)
        {
            using (var archive = new ZipArchive(saveInfo.FileSystem.OpenWriteNew(saveInfo.FilePath), ZipArchiveMode.Create, false, Encoding.UTF8))
            {
                SaveMetadata(archive, saveInfo);
                var assets = new Dictionary<string, IAsset>();
                var generatedResources = new List<GeneratedResourceSource>();
                var typeAliases = new Dictionary<Type, string>();
                if (saveInfo.IncludeEditingWorld)
                    SaveWorld(archive, saveInfo.World, "world.json", typeAliases, assets, generatedResources);
                if (saveInfo.IncludeReadOnlyWorld)
                    SaveWorld(archive, saveInfo.ReadOnlyWorld, "readOnlyWorld.json", typeAliases, assets, generatedResources);
                SaveAssets(archive, saveInfo, assets, typeAliases);
                SaveGeneratedResources(archive, generatedResources, typeAliases);
                SaveTypeAliases(archive, typeAliases);
            }
        }

        private void SaveMetadata(ZipArchive archive, IFileSaveInfo saveInfo)
        {
            var metaEntry = archive.CreateEntry("meta.json");
            using (var writer = trwFactory.JsonWriter(metaEntry.Open()))
            using (var context = saveLoadFactory.MetaWriteContext(writer))
                context.Write(new SaveLoadMetadata
                {
                    Version = converterContainer.CurrentVersion,
                    IncludesEditingWorld = saveInfo.IncludeEditingWorld,
                    IncludesReadOnlyWorld = saveInfo.IncludeReadOnlyWorld
                });
        }

        private void SaveWorld(ZipArchive archive, IWorld world, string worldFileName, IDictionary<Type, string> typeAliases,
            Dictionary<string, IAsset> assets, List<GeneratedResourceSource> generatedResources)
        {
            var worldEntry = archive.CreateEntry(worldFileName);
            using (var writer = trwFactory.JsonWriter(worldEntry.Open()))
            using (var context = saveLoadFactory.WorldWriteContext(writer))
            {
                context.TypeAliases = typeAliases;
                context.Bag.Add(SaveLoadConstants.AssetDictBagKey, assets);
                context.Bag.Add(SaveLoadConstants.GeneratedResourcesBagKey, generatedResources);
                context.Write(world);
            }
        }

        private void SaveTypeAliases(ZipArchive archive, IDictionary<Type, string> typeAliases)
        {
            var aliasesEntry = archive.CreateEntry("typeAliases.json");
            using (var writer = trwFactory.JsonWriter(aliasesEntry.Open()))
            using (var context = saveLoadFactory.AliasesWriteContext(writer))
            {
                var rawAliases = typeAliases.ToDictionary(x => x.Value, x => x.Key.AssemblyQualifiedName);
                context.Write(rawAliases);
            }
        }

        private void SaveAssets(ZipArchive archive, IFileSaveInfo saveInfo, Dictionary<string, IAsset> assets, IDictionary<Type, string> typeAliases)
        {
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
            {
                context.TypeAliases = typeAliases;
                context.Write(assetInfos);
            }

            foreach (var asset in assets.Values)
            foreach (var localFilePath in saveInfo.GetAssetLocalFiles(asset))
            {
                var fileEntry = archive.CreateEntry(Path.Combine("Assets", asset.Name, localFilePath));
                using (var writeStream = fileEntry.Open())
                using (var readStream = saveInfo.ReadAssetFile(asset, localFilePath))
                    readStream.CopyTo(writeStream);
            }
        }

        private void SaveGeneratedResources(ZipArchive archive, IReadOnlyList<GeneratedResourceSource> generatedResources, IDictionary<Type, string> typeAliases)
        {
            var saveLoadInfos = new List<GeneratedResourceSaveLoadInfo>();
            
            var zipFileSystem = new ZipFileSystem(archive);
            for (var i = 0; i < generatedResources.Count; i++)
            {
                var source = generatedResources[i];
                var resource = source.Resource;
                var folderName = "r" + i;
                var fileName = resourceSavingService.SuggestFileName(resource, folderName);
                var path = $"GeneratedResources/{folderName}/{fileName}";
                saveLoadInfos.Add(new GeneratedResourceSaveLoadInfo(source.SaveLoadResourceType, path));
                resourceSavingService.SaveResource(resource, zipFileSystem, path);
            }

            var generatedResourceInfoEntry = archive.CreateEntry("generatedResourceInfos.json");
            using (var writer = trwFactory.JsonWriter(generatedResourceInfoEntry.Open()))
            using (var context = saveLoadFactory.GeneratedResourceInfoWriteContext(writer))
            {
                context.TypeAliases = typeAliases;
                context.Write(saveLoadInfos.ToArray());
            }
        }

        public void Load(IFileLoadInfo loadInfo)
        {
            using (var archive = new ZipArchive(loadInfo.FileSystem.OpenRead(loadInfo.FilePath), ZipArchiveMode.Read, false, Encoding.UTF8))
            {
                var metadata = LoadMetadata(archive);
                var typeAliases = LoadTypeAliases(archive, metadata);
                var assets = LoadAssets(archive, metadata, typeAliases, loadInfo);
                var generatedResources = LoadGeneratedResources(archive, metadata, typeAliases);
                if (metadata.IncludesReadOnlyWorld && (loadInfo.PreferReadOnly || !metadata.IncludesEditingWorld))
                {
                    var world = LoadWorld(archive, "readOnlyWorld.json", metadata, typeAliases, assets, generatedResources);
                    loadInfo.OnLoadedReadOnlyWorld(world);
                }
                else if (metadata.IncludesEditingWorld)
                {
                    var world = LoadWorld(archive, "world.json", metadata, typeAliases, assets, generatedResources);
                    loadInfo.OnLoadedWorld(world);
                }
                else
                {
                    loadInfo.OnLoadedNoWorld();
                }
            }
        }

        private SaveLoadMetadata LoadMetadata(ZipArchive archive)
        {
            var metaEntry = archive.GetEntry("meta.json") ?? throw new Exception("meta.json not found.");
            using (var reader = trwFactory.JsonReader(metaEntry.Open()))
            using (var context = saveLoadFactory.MetaReadContext(reader))
                return context.Read<SaveLoadMetadata>();
        }

        private IReadOnlyDictionary<string, Type> LoadTypeAliases(ZipArchive archive, SaveLoadMetadata metadata)
        {
            var aliasesEntry = archive.GetEntry("typeAliases.json") ?? throw new Exception("typeAliases.json not found.");
            using (var reader = trwFactory.JsonReader(aliasesEntry.Open()))
            using (var convertedReader = converterContainer.ConvertAliasesReader(reader, metadata.Version, converterContainer.CurrentVersion))
            using (var context = saveLoadFactory.AliasesReadContext(convertedReader))
            {
                var rawAliases = context.Read<Dictionary<string, string>>();
                return rawAliases.ToDictionary(x => x.Key, x => Type.GetType(x.Value));
            }
        }

        private IDictionary<string, IAsset> LoadAssets(ZipArchive archive, SaveLoadMetadata metadata, IReadOnlyDictionary<string, Type> typeAliases, IFileLoadInfo loadInfo)
        {
            AssetSaveLoadInfo[] assetInfos;
            var assetInfoEntry = archive.GetEntry("assetInfos.json") ?? throw new Exception("assetInfo.json not found.");
            using (var reader = trwFactory.JsonReader(assetInfoEntry.Open()))
            using (var convertedReader = converterContainer.ConvertAssetInfoReader(reader, metadata.Version, converterContainer.CurrentVersion))
            using (var context = saveLoadFactory.AssetsInfoReadContext(convertedReader, typeAliases))
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

            return assets;
        }

        private IReadOnlyList<IResource> LoadGeneratedResources(ZipArchive archive, SaveLoadMetadata metadata, IReadOnlyDictionary<string, Type> typeAliases)
        {
            GeneratedResourceSaveLoadInfo[] generatedResourceInfos;
            var generatedResourceInfoEntry = archive.GetEntry("generatedResourceInfos.json") ?? throw new Exception("generatedResourceInfos.json not found.");
            using (var reader = trwFactory.JsonReader(generatedResourceInfoEntry.Open()))
            using (var convertedReader = converterContainer.ConvertGeneratedResourceInfoReader(reader, metadata.Version, converterContainer.CurrentVersion))
            using (var context = saveLoadFactory.GeneratedResourceInfoReadContext(convertedReader, typeAliases))
                generatedResourceInfos = context.Read<GeneratedResourceSaveLoadInfo[]>();

            var generatedResources = new List<IResource>();
            var zipFileSystem = new ZipFileSystem(archive);
            foreach (var info in generatedResourceInfos)
                generatedResources.Add(resourceLoadingService.Load(info.Type, zipFileSystem, info.Path).WithSource(x => new GeneratedResourceSource(x, info.Type)));

            return generatedResources;
        }

        private IWorld LoadWorld(ZipArchive archive, string worldFileName, SaveLoadMetadata metadata, IReadOnlyDictionary<string, Type> typeAliases, IDictionary<string, IAsset> assets, IReadOnlyList<IResource> generatedResources)
        {
            IWorld world;
            var worldEntry = archive.GetEntry(worldFileName) ?? throw new Exception(worldFileName + " not found.");
            using (var reader = trwFactory.JsonReader(worldEntry.Open()))
            using (var convertedReader = converterContainer.ConvertWorldReader(reader, metadata.Version, converterContainer.CurrentVersion))
            using (var context = saveLoadFactory.WorldReadContext(convertedReader, typeAliases))
            {
                context.Bag.Add(SaveLoadConstants.AssetDictBagKey, assets);
                context.Bag.Add(SaveLoadConstants.GeneratedResourcesBagKey, generatedResources);
                world = context.Read<IWorld>();
            }

            return world;
        }
    }
}