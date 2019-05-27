using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Ext.Rendering.Ogl3.Caches;
using Clarity.Ext.Rendering.Ogl3.Drawers;
using Clarity.Ext.Rendering.Ogl3.Helpers;
using Clarity.Ext.Rendering.Ogl3.Uniforms;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.States.Blend;
using ObjectGL.Api.Objects.Framebuffers;
using OpenTK.Graphics.OpenGL4;
using CullFaceMode = ObjectGL.Api.Context.States.Rasterizer.CullFaceMode;
using FrontFaceDirection = ObjectGL.Api.Context.States.Rasterizer.FrontFaceDirection;

namespace Clarity.Ext.Rendering.Ogl3.Pipelining 
{
    public class SceneRenderer : ISceneRenderer
    {
        public const string IsStandardProp = "is-std";
        public const string DrawHighlightProp = "std-draw-highlight";
        public const string DrawBackgroundProp = "std-draw-background";
        public const string DrawSketchProp = "std-draw-sketch";

        private readonly IGraphicsInfra infra;
        private readonly IVisualElementHandlerContainer handlerContainer;
        private readonly ISkyboxDrawer skyboxDrawer;
        private readonly IBlurDrawer blurDrawer;
        private readonly ISketchDrawer sketchDrawer;
        private readonly IVeilDrawer veilDrawer;
        private readonly IHighlightDrawer highlightDrawer;

        private readonly List<Pair<RenderQueueItem, IVisualElementHandler>> regularQueue;
        private readonly List<Pair<RenderQueueItem, IVisualElementHandler>> focusedQueue;
        private readonly List<Pair<RenderQueueItem, IVisualElementHandler>> overlayQueue;
        private readonly List<Pair<RenderQueueItem, IVisualElementHandler>> opaqueSubqueue;
        private readonly List<Tuple3<RenderQueueItem, IVisualElementHandler, float>> transparentSubqueue;

        public SceneRenderer(IGraphicsInfra infra, ISkyboxDrawer skyboxDrawer, 
            IVisualElementHandlerContainer handlerContainer, IBlurDrawer blurDrawer, 
            ISketchDrawer sketchDrawer, IVeilDrawer veilDrawer, IHighlightDrawer highlightDrawer)
        {
            this.infra = infra;
            this.skyboxDrawer = skyboxDrawer;
            this.handlerContainer = handlerContainer;
            this.blurDrawer = blurDrawer;
            this.sketchDrawer = sketchDrawer;
            this.veilDrawer = veilDrawer;
            this.highlightDrawer = highlightDrawer;

            regularQueue= new List<Pair<RenderQueueItem, IVisualElementHandler>>();
            focusedQueue= new List<Pair<RenderQueueItem, IVisualElementHandler>>();
            overlayQueue= new List<Pair<RenderQueueItem, IVisualElementHandler>>();
            opaqueSubqueue= new List<Pair<RenderQueueItem, IVisualElementHandler>>();
            transparentSubqueue= new List<Tuple3<RenderQueueItem, IVisualElementHandler, float>>();
        }

        public bool SatisfiesRequirements(IPropertyBag pipelineRequirements)
        {
            return pipelineRequirements.SearchValue<bool>(IsStandardProp);
        }

        public void Execute(IScene scene, ICamera camera, IOffScreen offScreen, float timestamp, IPropertyBag settings)
        {
            var drawBackground = settings.SearchValue<bool>(DrawBackgroundProp);
            var drawHighlight = settings.SearchValue<bool>(DrawHighlightProp);
            var drawSketch = settings.SearchValue<bool>(DrawSketchProp);

            if (drawBackground)
                offScreen.Framebuffer.ClearColor(0, scene.BackgroundColor.ToOgl());
            offScreen.Framebuffer.ClearDepthStencil(DepthStencil.Both, 1f, 0);

            var glContext = infra.GlContext;
            var commonObjects = infra.CommonObjects;

            glContext.States.ScreenClipping.United.Viewport.Set(offScreen.Width, offScreen.Height);

            var aspectRatio = GraphicsHelper.AspectRatio(offScreen.Width, offScreen.Height);
            var viewFrame = camera.GetGlobalFrame();
            var viewProjMat = viewFrame.GetViewMat() * camera.GetProjectionMat(aspectRatio);

            if (drawBackground && scene.Skybox != null)
            {
                var glTextureCubemap = scene.Skybox.CacheContainer.GetOrAddCache(Tuples.Pair(infra, scene.Skybox), x => new SkyboxCache(x.First, x.Second)).GetGlTextureCubemap();
                skyboxDrawer.Draw(glTextureCubemap, viewFrame, camera.GetFov(), aspectRatio);
            }

            void FillForSubtree(ISceneNode subtreeRoot, bool alreadyFocus, bool alreadyHighlighted, bool alreadyOverlay)
            {
                var effects = subtreeRoot
                    .SearchComponents<IVisualComponent>()
                    .SelectMany(x => x.GetVisualEffects());
                foreach (var effect in effects)
                    switch (effect)
                    {
                        case FocusVisualEffect _:
                            alreadyFocus = true;
                            break;
                        case HighlightVisualEffect _:
                            alreadyHighlighted = true;
                            break;
                    }
                var elements = subtreeRoot
                    .SearchComponents<IVisualComponent>()
                    .SelectMany(x => x.GetVisualElements());
                foreach (var element in elements.Where(x => !x.Hide))
                {
                    var handler = handlerContainer.ChooseHandler(element);
                    var queueItem = new RenderQueueItem(element, handler, subtreeRoot, alreadyHighlighted);
                    if (alreadyOverlay)
                        overlayQueue.Add(Tuples.Pair(queueItem, handler));
                    else if (alreadyFocus)
                        focusedQueue.Add(Tuples.Pair(queueItem, handler));
                    else
                        regularQueue.Add(Tuples.Pair(queueItem, handler));
                }

                foreach (var childNode in subtreeRoot.ChildNodes)
                    FillForSubtree(childNode, alreadyFocus, alreadyHighlighted, alreadyOverlay);
            }

            FillForSubtree(scene.Root, false, false, false);
            foreach (var auxuliaryNode in scene.AuxuliaryNodes)
                FillForSubtree(auxuliaryNode, false, false, true);

            

            glContext.States.Rasterizer.FrontFace.Set(FrontFaceDirection.Ccw);
            glContext.States.Rasterizer.CullFaceEnable.Set(false);
            glContext.States.Rasterizer.CullFace.Set(CullFaceMode.Back);
            glContext.States.DepthStencil.DepthTestEnable.Set(true);
            glContext.States.DepthStencil.DepthMask.Set(true);
            glContext.GL.Enable((int)All.FramebufferSrgb);

            commonObjects.CameraUb.SetData(viewProjMat);
            commonObjects.CameraExtraUb.SetData(viewFrame.Eye);
            var lightPos = viewFrame.Eye + 0.5f * viewFrame.Right + 0.5f * viewFrame.Up;
            commonObjects.LightUb.SetData(lightPos);
            commonObjects.GlobalUb.SetData(new GlobalUniform
            {
                ScreenWidth = offScreen.Width,
                ScreenHeight = offScreen.Height,
                Time = timestamp
            });

            commonObjects.TransformUb.Bind(glContext, 0);
            commonObjects.CameraUb.Bind(glContext, 1);
            commonObjects.CameraExtraUb.Bind(glContext, 2);
            commonObjects.LightUb.Bind(glContext, 3);
            commonObjects.MaterialUb.Bind(glContext, 4);
            commonObjects.GlobalUb.Bind(glContext, 5);

            RenderSceneSubset(camera, aspectRatio, regularQueue);
            
            if (focusedQueue.HasItems())
            {
                offScreen.Resolve();
                glContext.Bindings.Framebuffers.Draw.Set(offScreen.Framebuffer);
                blurDrawer.Draw(offScreen.ResolvedTex);
                offScreen.Framebuffer.ClearDepthStencil(DepthStencil.Depth, 1f, 0);
                RenderSceneSubset(camera, aspectRatio, focusedQueue);
            }
            if (drawHighlight)
                highlightDrawer.Draw(offScreen.Framebuffer, offScreen.DepthBuffer);

            offScreen.Framebuffer.ClearDepthStencil(DepthStencil.Depth, 1f, 0);
            RenderSceneSubset(camera, aspectRatio, overlayQueue);

            if (drawSketch)
                sketchDrawer.Draw();

            if (camera.VeilColor.A > 0)
                veilDrawer.Draw(camera.VeilColor);
            
            regularQueue.Clear();
            focusedQueue.Clear();
            overlayQueue.Clear();
        }

        private void RenderSceneSubset(ICamera camera, float aspectRatio, IEnumerable<Pair<RenderQueueItem, IVisualElementHandler>> queueItems)
        {
            var glContext = infra.GlContext;

            foreach (var queueItemPair in queueItems)
            {
                var queueItem = queueItemPair.First;
                var handler = queueItemPair.Second;
                if (!handler.HasTransparency(queueItem))
                {
                    opaqueSubqueue.Add(queueItemPair);
                }
                else
                {
                    var cameraDistSq = handler.GetCameraDistSq(queueItem,camera);
                    transparentSubqueue.Add(Tuples.Tuple(queueItem, handler, cameraDistSq));
                }
            }

            foreach (var queueItemPair in opaqueSubqueue)
            {
                var queueItem = queueItemPair.First;
                var handler = queueItemPair.Second;
                handler.Draw(queueItem, camera, aspectRatio);
            }

            glContext.States.DepthStencil.DepthMask.Set(false);
            glContext.States.Blend.BlendEnable.Set(true);
            glContext.States.Blend.United.Equation.Set(BlendMode.Add);
            glContext.States.Blend.United.Function.Set(BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);

            transparentSubqueue.Sort((x, y) => -Comparer<float>.Default.Compare(x.Item2, y.Item2));

            foreach (var queueItemTuple in transparentSubqueue)
            {
                var queueItem = queueItemTuple.Item0;
                var handler = queueItemTuple.Item1;
                handler.Draw(queueItem, camera, aspectRatio);
            }

            glContext.States.Blend.BlendEnable.Set(false);
            glContext.States.DepthStencil.DepthMask.Set(true);
            
            opaqueSubqueue.Clear();
            transparentSubqueue.Clear();
        }
    }
}