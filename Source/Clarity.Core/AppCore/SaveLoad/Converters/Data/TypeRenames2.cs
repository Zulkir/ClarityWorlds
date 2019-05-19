using System.Collections.Generic;

namespace Clarity.Core.AppCore.SaveLoad.Converters.Data
{
    public static class TypeRenames2
    {
        public static IReadOnlyDictionary<string, SaveLoadRenamedTypeDescription> Renames { get; } = new Dictionary<string, SaveLoadRenamedTypeDescription>
        {
            //{"Node", "Tzomet"},
            //{"JustNodeComponent", "AbstractNodeComponent"}
        };
    }
}