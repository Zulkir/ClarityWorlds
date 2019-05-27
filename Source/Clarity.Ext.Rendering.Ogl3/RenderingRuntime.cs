using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Engine.Gui;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Ext.Rendering.Ogl3.Helpers;
using Clarity.Ext.Rendering.Ogl3.Pipelining;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Images;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class RenderingRuntime : IRenderingRuntime
    {
        private const int MsaaCount = 4;
        private const float OffscreenTtl = 2f;

        private readonly IGraphicsInfra infra;
        private readonly ISceneRendererPool sceneRendererPool;
        private readonly IOffScreenContainer offScreenContainer;
        private readonly List<object> offScreenCacheDeathNote;
        private readonly Dictionary<Tuple3<bool>, IPropertyBag> sceneRendererSettingsCache;

        private readonly IContext glContext;
        private readonly IFramebuffer rttFramebuffer;

        public RenderingRuntime(IGraphicsInfra infra, ISceneRendererPool sceneRendererPool, IOffScreenContainer offScreenContainer)
        {
            this.infra = infra;
            this.sceneRendererPool = sceneRendererPool;
            this.offScreenContainer = offScreenContainer;
            offScreenCacheDeathNote = new List<object>();
            sceneRendererSettingsCache = new Dictionary<Tuple3<bool>, IPropertyBag>();

            glContext = infra.GlContext;
            rttFramebuffer = glContext.Create.Framebuffer();
        }

        private IPropertyBag GetSceneRendererSettings(bool drawBackground, bool drawHighlight, bool drawSketch)
        {
            var key = Tuples.SameTypeTuple(drawBackground, drawHighlight, drawSketch);
            return sceneRendererSettingsCache.GetOrAdd(key, x =>
            {
                var settings = new PropertyBag();
                settings.SetValue(SceneRenderer.IsStandardProp, true);
                settings.SetValue(SceneRenderer.DrawBackgroundProp, x.Item0);
                settings.SetValue(SceneRenderer.DrawHighlightProp, x.Item1);
                settings.SetValue(SceneRenderer.DrawSketchProp, x.Item2);
                return settings;
            });
        }

        public void RenderToContext(IRenderGuiControl renderControl, float timestamp)
        {
            foreach (var viewport in renderControl.Viewports)
            {
                var offScreen = offScreenContainer.Get(this, viewport, viewport.Width, viewport.Height, MsaaCount, OffscreenTtl);
                for (var i = 0; i < viewport.View.Layers.Count; i++)
                {
                    var layer = viewport.View.Layers[i];
                    var isFirstLayer = i == 0;
                    var isLastLayer = i == viewport.View.Layers.Count - 1;
                    var sceneRendererSettings = GetSceneRendererSettings(isFirstLayer, true, isLastLayer);
                    var sceneRenderer = sceneRendererPool.Allocate(sceneRendererSettings);
                    sceneRenderer.Execute(layer.VisibleScene, layer.Camera, offScreen, timestamp, sceneRendererSettings);
                    sceneRendererPool.Return(sceneRenderer);
                }
                glContext.Actions.BlitFramebuffer(offScreen.Framebuffer, null,
                    0, 0, 
                    viewport.Width, viewport.Height,
                    viewport.Left, renderControl.Height - viewport.Top, 
                    viewport.Left + viewport.Width, renderControl.Height - (viewport.Top + viewport.Height),
                    ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            }

            glContext.Infra.SwapBuffers();

            // todo: to Update
            offScreenCacheDeathNote.Clear();

            infra.MainThreadDisposer.DisposeOfAll();
        }

        public void RenderToTexture(ITexture2D glTexture, IReadOnlyList<IViewport> viewports, float timestamp)
        {
            rttFramebuffer.AttachTextureImage(FramebufferAttachmentPoint.Color0, glTexture, 0);
            foreach (var viewport in viewports)
            {
                var offScreen = offScreenContainer.Get(this, viewport, viewport.Width, viewport.Height, MsaaCount, OffscreenTtl);
                for (var i = 0; i < viewport.View.Layers.Count; i++)
                {
                    var layer = viewport.View.Layers[i];
                    var isFirstLayer = i == 0;
                    var isLastLayer = i == viewport.View.Layers.Count - 1;
                    var sceneRendererSettings = GetSceneRendererSettings(isFirstLayer, true, isLastLayer);
                    var sceneRenderer = sceneRendererPool.Allocate(sceneRendererSettings);
                    sceneRenderer.Execute(layer.VisibleScene, layer.Camera, offScreen, timestamp, sceneRendererSettings);
                    sceneRendererPool.Return(sceneRenderer);
                }
                glContext.Actions.BlitFramebuffer(offScreen.Framebuffer, rttFramebuffer,
                    0, 0, 
                    viewport.Width, viewport.Height,
                    viewport.Left, glTexture.Height - viewport.Top, 
                    viewport.Left + viewport.Width, glTexture.Height - (viewport.Top + viewport.Height),
                    ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            }
            glTexture.GenerateMipmap();
        }
    }
}