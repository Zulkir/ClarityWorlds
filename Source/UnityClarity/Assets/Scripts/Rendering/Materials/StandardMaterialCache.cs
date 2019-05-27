using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Assets.Scripts.Rendering.Materials
{
    public class StandardMaterialCache : IStandardMaterialCache
    {
        private readonly IUcRenderingInfra infra;
        private readonly Dictionary<StandardMaterialKey, MaterialWithUsageTracking> materials;
        private readonly List<StandardMaterialKey> deathNote;
        private float currentTimestamp;

        public float UnusedMaterialTtl { get; } = 2f;

        public StandardMaterialCache(IUcRenderingInfra infra, IEventRoutingService eventRoutingService)
        {
            this.infra = infra;
            materials = new Dictionary<StandardMaterialKey, MaterialWithUsageTracking>();
            deathNote= new List<StandardMaterialKey>();
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(IStandardMaterialCache), nameof(OnNewFrame), OnNewFrame);
        }

        public IEnumerable<Material> EnumerateAll()
        {
            return materials.Values.Select(x => x.Material);
        }

        public Material GetOrAddMaterial(StandardMaterialKey key)
        {
            var materialWithUsageTracking = materials.GetOrAdd(key, x => new MaterialWithUsageTracking(CreateMaterial(x)));
            materialWithUsageTracking.LastUsedTimestamp = currentTimestamp;
            return materialWithUsageTracking.Material;
        }

        private Material CreateMaterial(StandardMaterialKey key)
        {
            var cMaterial = key.CStandardMaterial;
            var uMaterial = new Material(infra.ClarityStandardShader);

            uMaterial.SetColor("_Color", cMaterial.DiffuseColor.ToUnity());
            uMaterial.SetTexture("_DiffuseMap", cMaterial.DiffuseMap?.CacheContainer.GetOrAddCache(cMaterial.DiffuseMap, x => new UcCgImageCache(x)).GetUnityTexture());
            uMaterial.SetTexture("_NormalMap", cMaterial.NormalMap?.CacheContainer.GetOrAddCache(cMaterial.NormalMap, x => new UcCgImageCache(x, true)).GetUnityTexture());
            uMaterial.SetInt("_UseTexture", cMaterial.DiffuseMap != null ? 1 : 0);
            uMaterial.SetInt("_UseNormalMap", (cMaterial.DiffuseMap != null && cMaterial.NormalMap != null) ? 1 : 0);

            uMaterial.SetInt("_Cull", (int)key.CStandardRenderState.CullFace.ToUnity());
            uMaterial.SetInt("_IgnoreLighting", cMaterial.IgnoreLighting ? 1 : 0);
            uMaterial.SetInt("_NoSpecular", cMaterial.NoSpecular ? 1 : 0);
            // todo

            return uMaterial;
        }

        private void OnNewFrame(INewFrameEvent evnt)
        {
            currentTimestamp = evnt.FrameTime.TotalSeconds;
            foreach (var kvp in materials)
            {
                if (kvp.Value.LastUsedTimestamp < currentTimestamp - UnusedMaterialTtl)
                    deathNote.Add(kvp.Key);
            }
            foreach (var key in deathNote)
            {
                UObject.Destroy(materials[key].Material);
                materials.Remove(key);
            }
            deathNote.Clear();
        }
    }
}