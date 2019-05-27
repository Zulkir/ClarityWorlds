using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.App.Worlds.SaveLoad
{
    [TrwSerialize]
    public struct GeneratedResourceSaveLoadInfo
    {
        [TrwSerialize]
        public Type Type;

        [TrwSerialize]
        public string Path;

        public GeneratedResourceSaveLoadInfo(Type type, string path)
        {
            Type = type;
            Path = path;
        }
    }
}