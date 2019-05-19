using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helpers;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Gui;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Viewports;
using u = UnityEngine;

namespace Assets.Scripts.Rendering
{
    public class UcRenderService : IRenderService
    {
        private readonly u.Camera unityCamera;

        private readonly IUcRenderingInfra infra;
        private readonly IWindowingSystem windowingSystem;
        private readonly IWorldTreeService worldTreeService;
        private readonly IViewService viewService;

        public UcRenderService(IUcRenderingInfra infra, IWindowingSystem windowingSystem, IRenderLoopDispatcher loopDispatcher, IWorldTreeService worldTreeService, IViewService viewService)
        {
            this.infra = infra;
            this.windowingSystem = windowingSystem;
            this.viewService = viewService;
            this.worldTreeService = worldTreeService;
            unityCamera = u.GameObject.Find("Main Camera").GetComponent<u.Camera>();
            loopDispatcher.Render += frameTime =>
            {
                Render(this.windowingSystem.RenderControl, frameTime.TotalSeconds);
            };
        }

        public void Render(IRenderGuiControl target, float timestamp)
        {
            if (!(target is UcRenderGuiControl))
                throw new NotImplementedException();

            var viewport = viewService.RenderControl.Viewports.First();
            var view = viewport.View;
            var layer = view.Layers.First();
            var camera = layer.Camera;
            var aScene = layer.VisibleScene;
            var viewFrame = camera.GetGlobalFrame();
            // todo: use skybox when necessary
            unityCamera.clearFlags = u.CameraClearFlags.SolidColor;
            unityCamera.backgroundColor = aScene.BackgroundColor.ToUnity();
            unityCamera.transform.localPosition = viewFrame.Eye.ToUnity(true);
            unityCamera.transform.localRotation = Quaternion.RotationToFrame(viewFrame.Right, viewFrame.Up).ToUnity(false);

            var sceneRoot = aScene.Root;

            foreach (var node in sceneRoot.EnumerateSceneNodesDeep())
            {
                var visualAspect = node.SearchComponent<IVisualComponent>();
                if (visualAspect == null)
                    continue;

                // todo: use explicit closure
                var nodeCache = node.CacheContainer.GetOrAddCache(() => new UcWorldNodeVisualCache(infra, node));
                nodeCache.PrepareUnityObjectsForRendering();
            }
        }

        public IImage CreateRenderTargetImage(IntSize2 size)
        {
            throw new NotImplementedException();
            //return new Ogl3TextureImage(ResourceVolatility.Stable, infra, size, false);
        }

        public void Render(IImage target, IReadOnlyList<IViewport> viewports, float timestamp)
        {
            throw new NotImplementedException();
        }
    }
}