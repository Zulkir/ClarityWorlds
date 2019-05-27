using System;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;
using Clarity.Ext.Rendering.Ogl3.Helpers;
using ObjectGL.Api;
using ObjectGL.Api.Context.States.DepthStencil;
using ObjectGL.Api.Objects.Framebuffers;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Resources.Images;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;
using BeginMode = ObjectGL.Api.Context.Actions.BeginMode;
using BufferTarget = ObjectGL.Api.Objects.Resources.Buffers.BufferTarget;
using BufferUsageHint = ObjectGL.Api.Objects.Resources.Buffers.BufferUsageHint;
using StencilFunction = ObjectGL.Api.Context.States.DepthStencil.StencilFunction;
using VertexAttribPointerType = ObjectGL.Api.Objects.VertexArrays.VertexAttribPointerType;

namespace Clarity.Ext.Rendering.Ogl3.Drawers
{
    public class HighlightDrawer : IHighlightDrawer
    {
        private const float OffScreenTtl = 2;

        private readonly IGraphicsInfra infra;
        private readonly IOffScreenContainer offScreenContainer;
        private readonly IBlurDrawer blurDrawer;
        private readonly IBleedDrawer bleedDrawer;
        private readonly IQuadDrawer quadDrawer;

        private readonly IShaderProgram program;
        private readonly IVertexArray vao;
        private readonly IFramebuffer drawSpotFramebuffer;

        public unsafe HighlightDrawer(IGraphicsInfra infra, IBlurDrawer blurDrawer, IQuadDrawer quadDrawer, IBleedDrawer bleedDrawer, IOffScreenContainer offScreenContainer)
        {
            this.infra = infra;
            this.blurDrawer = blurDrawer;
            this.quadDrawer = quadDrawer;
            this.bleedDrawer = bleedDrawer;
            this.offScreenContainer = offScreenContainer;

            #region Shader Text
            const string vertexShaderText =
                @"#version 150
in vec4 in_position;

void main()
{
    gl_Position = in_position;
}
";

            const string fragmentShaderText =
                @"#version 150
out vec4 out_color;

void main()
{
    out_color = vec4(1, 1, 1, 1);
}
";
            #endregion

            var vs = infra.GlContext.Create.VertexShader(vertexShaderText);
            var fs = infra.GlContext.Create.FragmentShader(fragmentShaderText);
            program = infra.GlContext.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = vs.EnumSelf(),
                FragmentShaders = fs.EnumSelf(),
                VertexAttributeNames = new[] {"in_position"}
            });

            var vertices = new[]
            {
                new Vector4(-1, 1, 0.5f, 1),
                new Vector4(-1, -1, 0.5f, 1),
                new Vector4(1, 1, 0.5f, 1),
                new Vector4(1, -1, 0.5f, 1)
            };

            IBuffer vb; 
            fixed (Vector4* pVertices = vertices)
                vb = infra.GlContext.Create.Buffer(BufferTarget.Array, vertices.Length * sizeof(Vector4), BufferUsageHint.StaticDraw, (IntPtr)pVertices);

            vao = infra.GlContext.Create.VertexArray();
            vao.SetVertexAttributeF(0, vb, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, sizeof(Vector4), 0);
            
            drawSpotFramebuffer = infra.GlContext.Create.Framebuffer();
        }

        public void Draw(IFramebuffer targetFramebuffer, IRenderbuffer depthStencil)
        {
            infra.GlContext.States.DepthStencil.StencilTestEnable.Set(true);
            infra.GlContext.States.Blend.BlendEnable.Set(false);
            infra.GlContext.States.DepthStencil.DepthMask.Set(false);
            infra.GlContext.States.DepthStencil.DepthTestEnable.Set(false);
            infra.GlContext.States.DepthStencil.Back.StencilWriteMask.Set(0xff);
            infra.GlContext.States.DepthStencil.Front.StencilWriteMask.Set(0xff);
            infra.GlContext.States.Rasterizer.CullFaceEnable.Set(false);

            var offScreen = offScreenContainer.Get(this, depthStencil, depthStencil.Width, depthStencil.Height, depthStencil.Samples, OffScreenTtl);
            drawSpotFramebuffer.AttachRenderbuffer(FramebufferAttachmentPoint.Color0, offScreen.ColorBuffer);
            drawSpotFramebuffer.AttachRenderbuffer(FramebufferAttachmentPoint.DepthStencil, depthStencil);
            drawSpotFramebuffer.ClearColor(0, new Color4(0, 0, 0, 0));
            infra.GlContext.Bindings.Framebuffers.Draw.Set(drawSpotFramebuffer);

            infra.GlContext.Bindings.Program.Set(program);
            infra.GlContext.Bindings.VertexArray.Set(vao);

            SetStencilFunc(StencilFunction.Equal);
            infra.GlContext.Actions.Draw.Arrays(BeginMode.TriangleStrip, 0, 4);
            offScreen.Resolve();
            infra.GlContext.Bindings.Framebuffers.Draw.Set(offScreen.Framebuffer);
            SetStencilFunc(StencilFunction.Always);

            bleedDrawer.Draw(offScreen.ResolvedTex, Common.Numericals.Colors.Color4.Orange);
            offScreen.Resolve();
            infra.GlContext.Bindings.Framebuffers.Draw.Set(targetFramebuffer);
            SetStencilFunc(StencilFunction.Notequal);
            quadDrawer.Draw(offScreen.ResolvedTex);
            //quadDrawer.Draw(Common.Numericals.Colors.Color4.Blue);
            SetStencilFunc(StencilFunction.Always);

            infra.GlContext.States.DepthStencil.StencilTestEnable.Set(false);
            infra.GlContext.States.DepthStencil.DepthMask.Set(true);
            infra.GlContext.States.DepthStencil.DepthTestEnable.Set(true);
        }

        private void SetStencilFunc(StencilFunction func)
        {
            infra.GlContext.States.DepthStencil.Front.StencilFunctionSettings.Set(new StencilFunctionSettings
            {
                Function = func,
                Mask = 0xff,
                Reference = 1
            });
            infra.GlContext.States.DepthStencil.Back.StencilFunctionSettings.Set(new StencilFunctionSettings
            {
                Function = func,
                Mask = 0xff,
                Reference = 1
            });
        }
    }
}