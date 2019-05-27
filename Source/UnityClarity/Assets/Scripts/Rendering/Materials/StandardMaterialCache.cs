using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Elements.Materials;
using UnityEngine;
using UnityEngine.Rendering;
using UObject = UnityEngine.Object;

namespace Assets.Scripts.Rendering.Materials
{
    public class StandardMaterialCache : IStandardMaterialCache
    {
        private readonly IUcRenderingInfra infra;
        private readonly Dictionary<StandardMaterialKey, MaterialWithUsageTracking> materials;
        private readonly List<StandardMaterialKey> deathNote;
        private float currentTimestamp;

        public Material InvisibleMaterial { get; }
        public float UnusedMaterialTtl { get; } = 2f;


        public StandardMaterialCache(IUcRenderingInfra infra, IEventRoutingService eventRoutingService)
        {
            this.infra = infra;
            materials = new Dictionary<StandardMaterialKey, MaterialWithUsageTracking>();
            deathNote= new List<StandardMaterialKey>();
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(IStandardMaterialCache), nameof(OnNewFrame), OnNewFrame);
            InvisibleMaterial = new Material(infra.ClarityInvisibleShader);
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
            var uMaterial = new Material(infra.ClarityStandardShader);
            SetMaterialOwnProperties(key, uMaterial);
            return uMaterial;
        }

        private static void SetMaterialOwnProperties(StandardMaterialKey key, Material uMaterial)
        {
            var cMaterial = key.CStandardMaterial;
            var cRenderState = key.CStandardRenderState;

            // todo: optimize redundant sets away

            uMaterial.SetColor("_Color", cMaterial.DiffuseColor.ToUnity());
            uMaterial.SetTexture("_DiffuseMap", cMaterial.DiffuseMap?.CacheContainer.GetOrAddCache(cMaterial.DiffuseMap, x => new UcCgImageCache(x)).GetUnityTexture());
            uMaterial.SetTexture("_NormalMap", cMaterial.NormalMap?.CacheContainer.GetOrAddCache(cMaterial.NormalMap, x => new UcCgImageCache(x, true)).GetUnityTexture());
            uMaterial.SetInt("_UseTexture", cMaterial.DiffuseMap != null ? 1 : 0);
            uMaterial.SetInt("_UseNormalMap", (cMaterial.DiffuseMap != null && cMaterial.NormalMap != null) ? 1 : 0);

            uMaterial.SetInt("_Cull", (int)cRenderState.CullFace.ToUnity());
            uMaterial.SetInt("_IgnoreLighting", cMaterial.IgnoreLighting ? 1 : 0);
            uMaterial.SetInt("_NoSpecular", cMaterial.NoSpecular ? 1 : 0);

            if (!cMaterial.HasTransparency)
            {
                uMaterial.SetInt("_ZWrite", 1);
                uMaterial.SetInt("_BlendSrc", (int)BlendMode.One);
                uMaterial.SetInt("_BlendDst", (int)BlendMode.Zero);
                uMaterial.renderQueue = (int)RenderQueue.Geometry;
            }
            else
            {
                uMaterial.SetInt("_ZWrite", 0);
                uMaterial.SetInt("_BlendSrc", (int)BlendMode.SrcAlpha);
                uMaterial.SetInt("_BlendDst", (int)BlendMode.OneMinusSrcAlpha);
                uMaterial.renderQueue = (int)RenderQueue.Transparent;
            }

            if (cMaterial.HighlightEffect == HighlightEffect.Pulsating)
            {
                uMaterial.SetInt("_IsPulsating", 1);
                uMaterial.SetColor("_PulsatingColor", new Color(1.0f, 0.5f, 0.0f));
            }
            else
            {
                uMaterial.SetInt("_IsPulsating", 0);
            }

            // todo
        }

        private void OnNewFrame(INewFrameEvent evnt)
        {
            currentTimestamp = evnt.FrameTime.TotalSeconds;
            foreach (var kvp in materials)
                if (kvp.Value.LastUsedTimestamp >= currentTimestamp - UnusedMaterialTtl)
                    SetMaterialOwnProperties(kvp.Key, kvp.Value.Material);
                else
                    deathNote.Add(kvp.Key);
            foreach (var key in deathNote)
            {
                UObject.Destroy(materials[key].Material);
                materials.Remove(key);
            }
            deathNote.Clear();
        }
    }
}