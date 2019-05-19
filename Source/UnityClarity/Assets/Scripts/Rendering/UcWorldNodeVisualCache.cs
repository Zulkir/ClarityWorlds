using System;
using System.Collections.Generic;
using Assets.Scripts.Helpers;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.Caching;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.BasicGraphics;
using Clarity.Engine.Visualization.BasicGraphics.Materials;
using Clarity.Engine.Visualization.Components;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Rendering
{
    public class UcWorldNodeVisualCache : ICache
    {
        private readonly IUcRenderingInfra infra;
        private readonly GameObject parentObj;
        private readonly List<Pair<GameObject, Material>> unityObjects;
        
        public ISceneNode Node { get; }
        public bool IsDisposed { get; private set; }

        public UcWorldNodeVisualCache(IUcRenderingInfra infra, ISceneNode node)
        {
            this.infra = infra;
            Node = node;
            parentObj = GameObject.Find("VisualObjects");
            unityObjects = new List<Pair<GameObject, Material>>();
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public void OnMasterEvent(object message) { }

        public void PrepareUnityObjectsForRendering()
        {
            var aVisual = Node.GetComponent<IVisualComponent>();
            int i = 0;
            foreach (var visualElem in aVisual.GetVisualElements())
            {
                var unityPair = GetOrAddUnityPair(i);
                var unityObj = unityPair.First;
                var unityMaterial = unityPair.Second;
                if (!(visualElem is ICgModelVisualElement))
                {
                    unityObj.SetActive(false);
                    continue;
                }
                var modelElem = (ICgModelVisualElement)visualElem;

                var globalTransform = modelElem.Transform * Node.GlobalTransform;
                unityObj.transform.localPosition = globalTransform.Offset.ToUnity(true);
                unityObj.transform.localRotation = globalTransform.Rotation.ToUnity(true);
                unityObj.transform.localScale = globalTransform.Scale * modelElem.NonUniformScale.ToUnity(false);

                var model = modelElem.Model;
                var modelCache = model.CacheContainer.GetOrAddCache(model, m => new UcCgModelCache(m));
                var mesh = modelCache.GetUnityMesh();
                var meshFilter = unityObj.GetComponent<MeshFilter>();
                meshFilter.mesh = mesh;
                var meshRenderer = unityObj.GetComponent<MeshRenderer>();
                meshRenderer.receiveShadows = false;
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;

                var cMaterial = modelElem.Material;
                if (!(cMaterial is IStandardMaterial))
                {
                    SetMaterial(i, null);
                    continue;
                }
                var cStdMaterial = (IStandardMaterial)cMaterial;

                var baseMaterial = cStdMaterial.IgnoreLighting 
                    ? cStdMaterial.DiffuseTextureSource is ISingleColorPixelSource 
                        ? infra.DefaultUnlitColMaterial 
                        : infra.DefaultUnlitTexMaterial
                    : infra.DefaultLitMaterial;

                if (unityMaterial == null)
                {
                    unityMaterial = new Material(baseMaterial);
                    SetMaterial(i, unityMaterial);
                }
                else if (unityMaterial.shader != baseMaterial.shader)
                {
                    Object.Destroy(unityMaterial);
                    unityMaterial = new Material(baseMaterial);
                    SetMaterial(i, unityMaterial);
                }

                if (cStdMaterial.DiffuseTextureSource is ISingleColorPixelSource)
                {
                    unityMaterial.mainTexture = null;
                    unityMaterial.color = ((ISingleColorPixelSource)cStdMaterial.DiffuseTextureSource).Color.ToUnity();
                }
                else if (cStdMaterial.DiffuseTextureSource is IImage)
                {
                    var image = (IImage)cStdMaterial.DiffuseTextureSource;
                    var imageCache = image.CacheContainer.GetOrAddCache(image, x => new UcCgImageCache(x));
                    var texture = imageCache.GetUnityTexture();
                    unityMaterial.mainTexture = texture;
                }
                else
                {
                    throw new NotImplementedException();
                }

                i++;
            }
            var numUsedObjects = i;
            var numObjectsToRemove = unityObjects.Count - numUsedObjects;
            if (numObjectsToRemove > 0)
            {
                for (; i < unityObjects.Count; i++)
                    Object.Destroy(unityObjects[i].First);
                unityObjects.RemoveRange(numUsedObjects, numObjectsToRemove);
            }
        }

        private Pair<GameObject, Material> GetOrAddUnityPair(int index)
        {
            while (index >= unityObjects.Count)
                unityObjects.Add(new Pair<GameObject, Material>(CreateNewUnityObj(unityObjects.Count), null));
            return unityObjects[index];
        }

        private GameObject CreateNewUnityObj(int index)
        {
            var obj = new GameObject($"{Node.Name}_vis_{index}");
            obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
            obj.transform.SetParent(parentObj.transform);
            return obj;
        }

        private void SetMaterial(int index, Material material)
        {
            var obj = unityObjects[index].First;
            obj.GetComponent<MeshRenderer>().material = material;
            unityObjects[index] = new Pair<GameObject, Material>(obj, material);
        }
    }
}