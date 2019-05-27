using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Assets.Scripts.Rendering.Materials;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Media.Models.Explicit;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.Caching;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Rendering
{
    public class UcWorldNodeVisualCache : ICache
    {
        private readonly IStandardMaterialCache standardMaterialCache;

        private readonly GameObject parentObj;
        private readonly List<Pair<GameObject, MaterialPropertyBlock>> unityObjects;
        
        public ISceneNode Node { get; }

        public UcWorldNodeVisualCache(IDiContainer di, ISceneNode node)
        {
            standardMaterialCache = di.Get<IStandardMaterialCache>();
            Node = node;
            parentObj = di.Get<IGlobalObjectService>().VisualObjects;
            unityObjects = new List<Pair<GameObject, MaterialPropertyBlock>>();
        }

        public void Dispose()
        {
            // todo: MainThreadDisposer
            //foreach (var pair in unityObjects)
            //    Object.Destroy(pair.First);
            //unityObjects.Clear();
        }

        public void OnMasterEvent(object message) { }

        public void PrepareUnityObjectsForRendering(int cullingLayer)
        {
            var aVisual = Node.GetComponent<IVisualComponent>();
            int i = 0;
            foreach (var visualElem in aVisual.GetVisualElements())
            {
                var unityPair = GetOrAddUnityPair(i);
                var uObj = unityPair.First;
                var uPropBlock = unityPair.Second;
                if (visualElem.Hide)
                {
                    uObj.SetActive(false);
                    continue;
                }

                if (!(visualElem is IModelVisualElement))
                {
                    uObj.SetActive(false);
                    continue;
                }
                uObj.layer = cullingLayer;
                var cModelElem = (IModelVisualElement)visualElem;
                uObj.SetActive(true);

                var globalTransform = cModelElem.Transform * Node.GlobalTransform;
                uObj.transform.localPosition = globalTransform.Offset.ToUnity();
                uObj.transform.localRotation = globalTransform.Rotation.ToUnity();
                uObj.transform.localScale = globalTransform.Scale * cModelElem.NonUniformScale.ToUnity(false);

                var meshFilter = uObj.GetComponent<MeshFilter>();
                // todo: remove this stuff as soon as WalkableAreas are actually Walkable
                var meshCollider = uObj.GetComponent<MeshCollider>();
                var model = cModelElem.Model;
                if (model is IFlexibleModel)
                {
                    var flexibleModel = (IFlexibleModel)model;
                    var modelCache = flexibleModel.CacheContainer.GetOrAddCache(flexibleModel, m => new UcFlexibleModelCache(m));
                    var mesh = modelCache.GetUnityMesh();
                    meshFilter.sharedMesh = mesh;
                }
                else if (model is IExplicitModel)
                {
                    var explicitModel = (IExplicitModel)model;
                    var modelCache = explicitModel.CacheContainer.GetOrAddCache(explicitModel, m => new UcExplicitModelCache(m));
                    var mesh = modelCache.GetUnityMesh();
                    meshFilter.sharedMesh = mesh;
                }
                else
                {
                    throw new NotImplementedException();
                }

                meshCollider.sharedMesh =
                    cModelElem.ModelPartIndex == 0 &&
                    meshFilter.sharedMesh != null && 
                    meshFilter.sharedMesh.GetTopology(cModelElem.ModelPartIndex) == MeshTopology.Triangles && 
                    cullingLayer == 0 
                    ? meshFilter.sharedMesh 
                    : null;

                var meshRenderer = uObj.GetComponent<MeshRenderer>();
                meshRenderer.receiveShadows = false;
                meshRenderer.shadowCastingMode = ShadowCastingMode.Off;

                var cMaterial = cModelElem.Material;
                if (!(cMaterial is IStandardMaterial))
                {
                    uObj.SetActive(false);
                    continue;
                }
                var cStdMaterial = (IStandardMaterial)cMaterial;

                var cRenderState = cModelElem.RenderState;
                if (!(cRenderState is IStandardRenderState))
                {
                    uObj.SetActive(false);
                    continue;
                }
                var cStdRenderState = (IStandardRenderState)cRenderState;

                uObj.SetActive(true);

                var uMaterial = standardMaterialCache.GetOrAddMaterial(new StandardMaterialKey
                {
                    CStandardMaterial = cStdMaterial,
                    CStandardRenderState = cStdRenderState
                });

                // todo: check if there is a performance penalty for this sub-mesh solution and fix if there is
                var sharedMaterials = Enumerable.Range(0, model.PartCount).Select(x => standardMaterialCache.InvisibleMaterial).ToArray();
                sharedMaterials[cModelElem.ModelPartIndex] = uMaterial;
                meshRenderer.sharedMaterials = sharedMaterials;
                //uPropBlock.SetFloat("_Cull", (int)cModelElem.CullFace);

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

        private Pair<GameObject, MaterialPropertyBlock> GetOrAddUnityPair(int index)
        {
            while (index >= unityObjects.Count)
                unityObjects.Add(Tuples.Pair(CreateNewUnityObj(unityObjects.Count), new MaterialPropertyBlock()));
            return unityObjects[index];
        }

        private GameObject CreateNewUnityObj(int index)
        {
            var obj = new GameObject($"{Node.Name}_vis_{index}");
            obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
            obj.AddComponent<MeshCollider>();
            obj.transform.SetParent(parentObj.transform);
            return obj;
        }
    }
}