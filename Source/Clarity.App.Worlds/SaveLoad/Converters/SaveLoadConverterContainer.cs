using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.SaveLoad.Converters.Data;
using Clarity.App.Worlds.SaveLoad.Converters.Versions;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.App.Worlds.SaveLoad.Converters
{
    public class SaveLoadConverterContainer : ISaveLoadConverterContainer
    {
        private readonly Dictionary<int, Func<ISaveLoadVersionConverter>> worldConverters;
        private readonly Dictionary<int, Func<ISaveLoadVersionConverter>> assetInfoConvertersConverters;
        private readonly Dictionary<int, IReadOnlyDictionary<string, SaveLoadRenamedTypeDescription>> typeRenames;
        public int CurrentVersion => 4;

        public SaveLoadConverterContainer()
        {
            typeRenames = new Dictionary<int, IReadOnlyDictionary<string, SaveLoadRenamedTypeDescription>>
            {
                {2, TypeRenames2.Renames},
                {3, TypeRenames3.Renames},
                {4, EmptyTypeRenames.Renames}
            };

            worldConverters = new Dictionary<int, Func<ISaveLoadVersionConverter>>
            {
                {2, () => new SaveLoadConverter2()},
                {3, () => new SaveLoadConverter3()},
                {4, () => new WorldConverter4()}
            };

            assetInfoConvertersConverters = new Dictionary<int, Func<ISaveLoadVersionConverter>>
            {
                {2, () => new PassthroughSaveLoadConverter()},
                {3, () => new PassthroughSaveLoadConverter()}
            };
        }

        public ITrwReader ConvertAssetInfoReader(ITrwReader reader, int fromVersion, int toVersion)
        {
            if (fromVersion == toVersion)
                return reader;
            throw new NotImplementedException();
        }

        public ITrwReader ConvertGeneratedResourceInfoReader(ITrwReader reader, int fromVersion, int toVersion)
        {
            if (fromVersion == toVersion)
                return reader;
            throw new NotImplementedException();
        }

        public ITrwReader ConvertAliasesReader(ITrwReader reader, int fromVersion, int toVersion)
        {
            var result = reader;
            while (fromVersion < toVersion)
            {
                fromVersion++;
                result = new SaveLoadTypeAliasesConverter(result, typeRenames[fromVersion]);
            }
            return result;
        }
         
        public ITrwReader ConvertWorldReader(ITrwReader reader, int fromVersion, int toVersion)
        {
            if (fromVersion == toVersion)
                return reader;

            var converterChain = Enumerable.Range(fromVersion + 1, toVersion - fromVersion)
                .Select(x => worldConverters[x]())
                .ToArray();

            converterChain[0].ChainFrom(reader);
            for (int i = 1; i < converterChain.Length; i++)
                converterChain[i].ChainFrom(converterChain[i - 1]);

            return converterChain.Last().GetResultAsReader();
        }
    }
}