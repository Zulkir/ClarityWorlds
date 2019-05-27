using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Assets.Scripts.Infra;
using Assets.Scripts.Rendering.Materials;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Viewports;
using u = UnityEngine;

namespace Assets.Scripts.Rendering
{
    public class UcRenderService : IRenderService
    {
        private const int DisabledLayer = 8;
        private const int RttLayer = 9;

        private readonly IGlobalObjectService globalObjectService;

        private readonly IDiContainer di;
        private readonly IStandardMaterialCache standardMaterialCache;

        public UcRenderService(IDiContainer di, 
            IEventRoutingService eventRoutingService,
            IGlobalObjectService globalObjectService,
            IWindowingSystem windowingSystem,
            IStandardMaterialCache standardMaterialCache)
        {
            this.di = di;
            this.globalObjectService = globalObjectService;
            this.standardMaterialCache = standardMaterialCache;
            eventRoutingService.RegisterServiceDependency(typeof(IRenderService), typeof(IStandardMaterialCache));
            eventRoutingService.Subscribe<ILateUpdateEvent>(typeof(IRenderService), "OnLateUpdate", e =>
            {
                Render(windowingSystem.RenderControl, e.FrameTime.TotalSeconds);
            });
        }

        public void Render(IRenderGuiControl target, float timestamp)
        {
            if (!(target is UcRenderGuiControl))
                throw new NotImplementedException();

            // todo: support more than one viewport
            var viewport = target.Viewports.First();
            Render(viewport, globalObjectService.MainCamera, 0, timestamp);
        }

        public IImage CreateRenderTargetImage(IntSize2 size)
        {
            return new UcRenderTextureImage(size);
        }
        
        public void Render(IImage target, IReadOnlyList<IViewport> viewports, float timestamp)
        {
            var ucTarget = (UcRenderTextureImage)target;
            var viewport = viewports.First();
            var renderTexture = ucTarget.Texture;
            var uCamera = globalObjectService.RttCamera;
            uCamera.cullingMask = 1 << RttLayer;
            uCamera.targetTexture = renderTexture;
            Render(viewport, uCamera, RttLayer, timestamp);
            u.GL.invertCulling = true;
            uCamera.projectionMatrix = uCamera.projectionMatrix * u.Matrix4x4.Scale(new u.Vector3(1, -1, 1));
            uCamera.Render();
            uCamera.ResetProjectionMatrix();
            u.GL.invertCulling = false;
            uCamera.targetTexture = null;
        }

        private void Render(IViewport viewport, u.Camera uCamera, int cullingLayer, float timestamp)
        {
            uCamera.ResetProjectionMatrix();
            var view = viewport.View;
            foreach(var layer in view.Layers.Take(1))
            {
                var camera = layer.Camera;
                var aScene = layer.VisibleScene;
                var viewFrame = camera.GetGlobalFrame();

                if (!(camera is IFromUnityCamera))
                {
                    uCamera.transform.localPosition = viewFrame.Eye.ToUnity();
                    uCamera.transform.localRotation = Quaternion.RotationToFrame(viewFrame.Right, viewFrame.Up).ToUnity();
                    var projProps = camera.GetProjectionProps();
                    uCamera.nearClipPlane = projProps.ZNear;
                    uCamera.farClipPlane = projProps.ZFar;
                    switch (projProps.Type)
                    {
                        case CameraProjectionType.Perspective:
                            {
                                uCamera.orthographic = false;
                                uCamera.fieldOfView = u.Mathf.Rad2Deg * projProps.Fov;
                                break;
                            }
                        case CameraProjectionType.Orthographic:
                            {
                                uCamera.orthographic = true;
                                uCamera.orthographicSize = projProps.Scale;
                                break;
                            }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // todo: restore background colors and implement skyboxes

                var lightPos = viewFrame.Eye + 0.5f * viewFrame.Right + 0.5f * viewFrame.Up;

                foreach (var uStandardMaterial in standardMaterialCache.EnumerateAll())
                {
                    uStandardMaterial.SetVector("_CameraPosition", camera.GetEye().ToUnity());
                    uStandardMaterial.SetVector("_LightPosition", lightPos.ToUnity4());
                }

                foreach (var visualObject in globalObjectService.VisualObjects.transform.OfType<u.GameObject>())
                    visualObject.layer = DisabledLayer;

                var sceneRoot = aScene.Root;
                foreach (var node in sceneRoot.EnumerateSceneNodesDeep())
                {
                    var visualAspect = node.SearchComponent<IVisualComponent>();
                    if (visualAspect == null)
                        continue;
                    // todo: use explicit closure
                    var nodeCache = node.CacheContainer.GetOrAddCache(() => new UcWorldNodeVisualCache(di, node));
                    nodeCache.PrepareUnityObjectsForRendering(cullingLayer);
                }
            }
        }
    }
}