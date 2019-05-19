using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Core.AppCore.SaveLoad
{
    [TrwSerialize]
    public struct SaveLoadMetadata
    {
        [TrwSerialize]
        public int Version { get; set; }
    }
}