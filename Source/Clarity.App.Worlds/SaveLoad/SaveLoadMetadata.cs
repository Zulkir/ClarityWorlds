using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.App.Worlds.SaveLoad
{
    [TrwSerialize]
    public struct SaveLoadMetadata
    {
        [TrwSerialize]
        public int Version { get; set; }

        [TrwSerialize]
        public bool IncludesEditingWorld { get; set; }

        [TrwSerialize]
        public bool IncludesReadOnlyWorld { get; set; }
    }
}