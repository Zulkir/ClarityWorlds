using UnityEngine;

namespace Assets.Scripts.Rendering.Materials
{
    public class MaterialWithUsageTracking
    {
        public Material Material { get; }
        public float LastUsedTimestamp { get; set; }

        public MaterialWithUsageTracking(Material material)
        {
            Material = material;
        }
    }
}