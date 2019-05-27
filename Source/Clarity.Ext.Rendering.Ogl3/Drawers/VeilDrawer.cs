using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Ext.Rendering.Ogl3.Sugar;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.States.Blend;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;
using BeginMode = ObjectGL.Api.Context.Actions.BeginMode;
using BufferTarget = ObjectGL.Api.Objects.Resources.Buffers.BufferTarget;
using BufferUsageHint = ObjectGL.Api.Objects.Resources.Buffers.BufferUsageHint;
using VertexAttribPointerType = ObjectGL.Api.Objects.VertexArrays.VertexAttribPointerType;

namespace Clarity.Ext.Rendering.Ogl3.Drawers
{
    public class VeilDrawer : IVeilDrawer
    {
        private readonly IContext glContext;
        private readonly IShaderProgram shaderProgram;
        private readonly IVertexArray vao;
        private readonly UniformBufferSugar<Color4> colorUb;

        public unsafe VeilDrawer(IContext glContext)
        {
            this.glContext = glContext;

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

layout(std140) uniform ColorUb
{
    vec4 Color;
};

out vec4 out_color;

void main()
{
    out_color = Color;
}
";
            #endregion

            var vs = glContext.Create.VertexShader(vertexShaderText);
            var fs = glContext.Create.FragmentShader(fragmentShaderText);
            shaderProgram = glContext.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] { vs },
                FragmentShaders = new[] { fs },
                VertexAttributeNames = new[] { "in_position" },
                UniformBufferNames = new[] { "ColorUb" },
            });

            var vertices = new[]
            {
                new Vector4(-1, -1, 0, 1),
                new Vector4(-1, 1, 0, 1),
                new Vector4(1, -1, 0, 1),
                new Vector4(1, 1, 0, 1),
            };

            IBuffer vertexBuffer;
            fixed (Vector4* data = vertices)
                vertexBuffer = glContext.Create.Buffer(BufferTarget.Array, 4 * sizeof(float) * vertices.Length, BufferUsageHint.StaticDraw, (IntPtr)data);
            
            vao = glContext.Create.VertexArray();
            vao.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            colorUb = new UniformBufferSugar<Color4>(glContext);
        }

        public void Draw(Color4 color4)
        {
            glContext.Bindings.Program.Set(shaderProgram);
            glContext.Bindings.VertexArray.Set(vao);

            glContext.States.DepthStencil.DepthTestEnable.Set(false);

            glContext.States.Blend.BlendEnable.Set(true);
            glContext.States.Blend.United.Equation.Set(BlendMode.Add);
            glContext.States.Blend.United.Function.Set(BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);

            colorUb.SetData(color4);
            colorUb.Bind(glContext, 0);

            glContext.Actions.Draw.Arrays(BeginMode.TriangleStrip, 0, 4);

            glContext.States.Blend.BlendEnable.Set(false);
            glContext.States.DepthStencil.DepthTestEnable.Set(true);
        }
    }
}