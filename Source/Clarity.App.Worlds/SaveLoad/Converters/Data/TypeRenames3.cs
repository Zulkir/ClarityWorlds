using System.Collections.Generic;

namespace Clarity.App.Worlds.SaveLoad.Converters.Data
{
    public static class TypeRenames3
    {
        public static IReadOnlyDictionary<string, SaveLoadRenamedTypeDescription> Renames { get; } = new Dictionary<string, SaveLoadRenamedTypeDescription>
        {
            //{"Tzomet", "SceneNode" },
            //{"OldSphereAbstractNodeLayout", "SphereAbstractNodeLayout" }
        };
    }
}