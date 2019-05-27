using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Rendering.Materials
{
    public interface IStandardMaterialCache
    {
        Material InvisibleMaterial { get; }
        IEnumerable<Material> EnumerateAll();
        Material GetOrAddMaterial(StandardMaterialKey key);
        float UnusedMaterialTtl { get; }
    }
}