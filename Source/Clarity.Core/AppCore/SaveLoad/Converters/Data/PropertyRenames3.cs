using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Core.AppCore.SaveLoad.Converters.Data
{
    public static class PropertyRenames3
    {
        public static IReadOnlyDictionary<Pair<string, string>, string> Renames { get; } = new Dictionary<Pair<string, string>, string>
        {
            {new Pair<string, string>("SceneComponent", "BackgroundClr"), "BackgroundColor"},
            {new Pair<string, string>("CommonWorldNodeComponent", "L0calTransform"), "LocalTransform"},
            {new Pair<string, string>("CommonWorldNodeComponent", "Prefers222D"), "Prefers2D"},
            {new Pair<string, string>("Transform", "ScAAle"), "Scale"}
        };
    }
}