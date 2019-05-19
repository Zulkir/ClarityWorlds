using System.Collections.Generic;

namespace Clarity.Core.AppCore.SaveLoad.Converters.Data
{
    public static class EmptyTypeRenames
    {
        public static IReadOnlyDictionary<string, SaveLoadRenamedTypeDescription> Renames { get; } =
            new Dictionary<string, SaveLoadRenamedTypeDescription>();
    }
}