using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Gui;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Viewports;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.States.Blend;
using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Images;
using BlitFramebufferFilter = ObjectGL.Api.Context.Actions.BlitFramebufferFilter;
using ClearBufferMask = ObjectGL.Api.Context.Actions.ClearBufferMask;
using CullFaceMode = ObjectGL.Api.Context.States.Rasterizer.CullFaceMode;
using FrontFaceDirection = ObjectGL.Api.Context.States.Rasterizer.FrontFaceDirection;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class RenderingRuntime : IRenderingRuntime
    {
        //private readonly IViewService viewService;

        private readonly IGraphicsInfra infra;
        private readonly IDrawableTextureCubeFactory drawableTextureCubeFactory;
        private readonly Dictionary<ISkybox, IDrawableTextureCube> textureCubeCache;
        private readonly SkyboxDrawer skyboxDrawer;
        private readonly VeilDrawer veilDrawer;
        private readonly BlurDrawer blurDrawer;
        private readonly ISketchDrawer sketchDrawer;
        private readonly ISceneRenderingContextFactory sceneRenderingContextFactory;
        private readonly ICommonObjects commonObjects;
        private readonly IVisualElementHandlerChoices visualElementHandlerChoices;
        private readonly OffScreen mainOffScreen;
        private readonly OffScreen rttOffScreen;

        private readonly IContext glContext;
        private readonly IFramebuffer rttFramebuffer;
        private List<ISceneNode> blurredNodes;
        private List<ISceneNode> focusedNodes;

        public RenderingRuntime(IGraphicsInfra infra, IDrawableTextureCubeFactory drawableTextureCubeFactory,
            ISketchDrawer sketchDrawer, ISceneRenderingContextFactory sceneRenderingContextFactory, ICommonObjects commonObjects, 
            IVisualElementHandlerChoices visualElementHandlerChoices)
        {
            this.infra = infra;
            this.drawableTextureCubeFactory = drawableTextureCubeFactory;
            this.sketchDrawer = sketchDrawer;
            this.sceneRenderingContextFactory = sceneRenderingContextFactory;
            this.commonObjects = commonObjects;
            this.visualElementHandlerChoices = visualElementHandlerChoices;

            glContext = infra.GlContext;

            textureCubeCache = new Dictionary<ISkybox, IDrawableTextureCube>();
            skyboxDrawer = new SkyboxDrawer(glContext);
            veilDrawer = new VeilDrawer(glContext);
            blurDrawer = new BlurDrawer(glContext);
            mainOffScreen = new OffScreen(infra);
            rttOffScreen = new OffScreen(infra);

            rttFramebuffer = glContext.Create.Framebuffer();

            blurredNodes = new List<ISceneNode>();
            focusedNodes = new List<ISceneNode>();
        }

        public void RenderToContext(IRenderGuiControl renderControl, float timestamp)
        {
            mainOffScreen.Prepare(renderControl.Width, renderControl.Height, 4);
            //glContext.GL.Enable((int)All.FramebufferSrgb);
            glContext.Bindings.Framebuffers.Draw.Set(mainOffScreen.Framebuffer);

            foreach (var viewport in renderControl.Viewports)
            {
                var firstLayer = true;
                foreach (var layer in viewport.View.Layers)
                {
                    var viewportInfo = new RenderViewportInfo(viewport, layer);
                    RenderViewport(mainOffScreen, viewportInfo, timestamp, firstLayer);
                    firstLayer = false;
                }
            }

            glContext.Actions.BlitFramebuffer(mainOffScreen.Framebuffer, null,
                0, renderControl.Height, renderControl.Width, 0,
                0, 0, renderControl.Width, renderControl.Height,
                ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            //glContext.GL.Disable((int)All.FramebufferSrgb);

            glContext.Infra.SwapBuffers();

            infra.MainThreadDisposer.DisposeOfAll();
        }

        public void RenderToTexture(ITexture2D glTexture, IReadOnlyList<IViewport> viewports, float timestamp)
        {
            rttOffScreen.Prepare(glTexture.Width, glTexture.Height, 1);
            //glContext.GL.Enable((int)All.FramebufferSrgb);
            glContext.Bindings.Framebuffers.Draw.Set(rttOffScreen.Framebuffer);

            foreach (var viewport in viewports)
            {
                var firstLayer = true;
                foreach (var layer in viewport.View.Layers)
                {
                    var viewportInfo = new RenderViewportInfo(viewport, layer);
                    RenderViewport(rttOffScreen, viewportInfo, timestamp, firstLayer);
                    firstLayer = false;
                }
            }

            rttFramebuffer.AttachTextureImage(FramebufferAttachmentPoint.Color0, glTexture, 0);

            glContext.Actions.BlitFramebuffer(rttOffScreen.Framebuffer, rttFramebuffer,
                0, glTexture.Height, glTexture.Width, 0,
                0, 0, glTexture.Width, glTexture.Height,
                ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            glTexture.GenerateMipmap();
        }

        private void RenderViewport(OffScreen offScreen, RenderViewportInfo viewportInfo, float timestamp, bool clearColor)
        {
            glContext.States.ScreenClipping.United.ScissorBox.Set(viewportInfo.ScissorBox);

            var scene = viewportInfo.Scene;

            if (scene == null)
            {
                glContext.States.Rasterizer.ScissorEnable.Set(true);
                glContext.Actions.ClearWindowColor(Color4.CornflowerBlue.ToOgl());
                glContext.Actions.ClearWindowDepthStencil(DepthStencil.Both, 1f, 0);
                glContext.States.Rasterizer.ScissorEnable.Set(false);
                return;
            }

            glContext.States.Rasterizer.ScissorEnable.Set(true);
            if (clearColor)
                offScreen.Framebuffer.ClearColor(0, scene.BackgroundColor.ToOgl());
            offScreen.Framebuffer.ClearDepthStencil(DepthStencil.Both, 1f, 0);
            glContext.States.Rasterizer.ScissorEnable.Set(false);

            glContext.States.ScreenClipping.United.Viewport.Set(viewportInfo.OglViewport);

            var camera = viewportInfo.Camera;
            var viewFrame = camera.GetGlobalFrame();
            var viewMat = viewFrame.GetViewMat();
            var projMat = camera.GetProjectionMat(viewportInfo.AspectRatio);
            var viewProjMat = viewMat * projMat;

            if (clearColor && scene.Skybox != null)
            {
                var drawableTexture = textureCubeCache.GetOrAdd(scene.Skybox, drawableTextureCubeFactory.Create);
                skyboxDrawer.Draw(drawableTexture, viewFrame, camera.GetFov(), viewportInfo.AspectRatio);
            }

            glContext.Bindings.Program.Set(commonObjects.StandardShaderProgram);

            glContext.States.DepthStencil.DepthTestEnable.Set(true);
            glContext.States.DepthStencil.DepthMask.Set(true);
            //glContext.States.Rasterizer.CullFaceEnable.Set(true);
            glContext.States.Rasterizer.CullFaceEnable.Set(false);
            glContext.States.Rasterizer.CullFace.Set(CullFaceMode.Back);
            glContext.States.Rasterizer.FrontFace.Set(FrontFaceDirection.Ccw);

            commonObjects.CameraUb.SetData(viewProjMat);
            commonObjects.CameraExtraUb.SetData(viewFrame.Eye);
            //var t = -viewFrame.Eye.Z / viewFrame.Forward.Z;
            //var target = viewFrame.Eye + t * viewFrame.Forward;
            //var lightPos = new Vector3(target.Xy / 2, 2f * viewFrame.Eye.Z);
            //var lightPos = new Vector3(1000, 1000, 500);
            var lightPos = viewFrame.Eye + 0.5f * viewFrame.Right + 0.5f * viewFrame.Up;
            commonObjects.LightUb.SetData(lightPos);

            var globalUbData = new GlobalUniform
            {
                ScreenWidth = viewportInfo.OglViewport.Width,
                ScreenHeight = viewportInfo.OglViewport.Height,
                Time = timestamp
            };
            commonObjects.GlobalUb.SetData(globalUbData);

            commonObjects.TransformUb.Bind(glContext, 0);
            commonObjects.CameraUb.Bind(glContext, 1);
            commonObjects.CameraExtraUb.Bind(glContext, 2);
            commonObjects.LightUb.Bind(glContext, 3);
            commonObjects.MaterialUb.Bind(glContext, 4);
            commonObjects.GlobalUb.Bind(glContext, 5);

            var sceneRoot = scene.Root;

            //blurredNodes.Clear();
            //focusedNodes.Clear();
            
            blurredNodes = new List<ISceneNode>();
            focusedNodes = new List<ISceneNode>();
            
            foreach (var node in sceneRoot.EnumerateSceneNodesDeep())
            {
                switch (scene.RenderStageDistribution.GetStage(node))
                {
                    case CgBasicRenderStage.Blurred:
                        blurredNodes.Add(node);
                        //focusedNodes.Add(node);
                        break;
                    case CgBasicRenderStage.Focused:
                    case CgBasicRenderStage.Gizmos:
                    case CgBasicRenderStage.Hud:
                        focusedNodes.Add(node);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (blurredNodes.Any())
            {
                RenderPass(camera, viewportInfo.AspectRatio, blurredNodes);
                offScreen.Resolve();
                glContext.Bindings.Framebuffers.Draw.Set(offScreen.Framebuffer);
                blurDrawer.Draw(offScreen.ResolvedTex, viewportInfo.OglViewport);
                offScreen.Framebuffer.ClearDepthStencil(DepthStencil.Both, 1f, 0);
            }
            
            RenderPass(camera, viewportInfo.AspectRatio, focusedNodes);

            sketchDrawer.Draw();

            if (camera.VeilColor.A > 0)
                veilDrawer.Draw(camera.VeilColor);
        }

        private void RenderPass(ICamera camera, float aspectRatio, IEnumerable<ISceneNode> nodes)
        {
            var context = sceneRenderingContextFactory.Create(null, camera, aspectRatio);

            foreach (var node in nodes)
            {
                var visualAspect = node.SearchComponent<IVisualComponent>();
                if (visualAspect == null)
                    continue;

                context.CurrentTraverseNode = node;

                foreach (var visual in visualAspect.GetVisualElements().Where(x => !x.Hide))
                {
                    var handler = visualElementHandlerChoices.ChooseHandler(visual);
                    handler.OnTraverse(context, visual);
                }
            }

            foreach (var stage in context.Stages)
            {
                context.CurrentStage = stage;
                if (stage.Queue == null)
                    continue;

                if (stage.Name == StandardRenderStageNames.Transparent)
                {
                    glContext.States.DepthStencil.DepthMask.Set(false);
                    glContext.States.Blend.BlendEnable.Set(true);
                    glContext.States.Blend.United.Equation.Set(BlendMode.Add);
                    glContext.States.Blend.United.Function.Set(BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);
                }

                foreach (var queueItem in stage.Queue.GetInOrder())
                    queueItem.Handler.OnDequeue(context, queueItem);

                if (stage.Name == StandardRenderStageNames.Transparent)
                {
                    glContext.States.Blend.BlendEnable.Set(false);
                    glContext.States.DepthStencil.DepthMask.Set(true);
                }
            }
        }
    }
}