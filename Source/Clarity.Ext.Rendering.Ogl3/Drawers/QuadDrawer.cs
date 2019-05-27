using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Ext.Rendering.Ogl3.Sugar;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Context.States.Blend;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Resources.Images;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;

namespace Clarity.Ext.Rendering.Ogl3.Drawers 
{
    public class QuadDrawer : IQuadDrawer 
    {
        private readonly IContext glContext;
        private readonly IShaderProgram colShaderProgram;
        private readonly IShaderProgram texShaderProgram;
        private readonly IVertexArray vao;
        private readonly UniformBufferSugar<Color4> colorUb;

        public unsafe QuadDrawer(IContext glContext)
        {
            this.glContext = glContext;

            #region Shader Text
            const string vertexShaderText =
@"#version 150

in vec4 in_vertex;

out vec2 v_tex_coord;

void main()
{
    gl_Position = vec4(in_vertex.xy, 0.5, 1.0);
    v_tex_coord = in_vertex.zw;
}
";

            const string colFragmentShaderText =
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

            const string texFragmentShaderText =
@"#version 150

uniform sampler2D Texture;

in vec2 v_tex_coord;

out vec4 out_color;

void main()
{
    out_color = texture(Texture, v_tex_coord);
}
";
            #endregion

            var vs = glContext.Create.VertexShader(vertexShaderText);
            var colFs = glContext.Create.FragmentShader(colFragmentShaderText);
            var texFs = glContext.Create.FragmentShader(texFragmentShaderText);
            colShaderProgram = glContext.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] { vs },
                FragmentShaders = new[] { colFs },
                VertexAttributeNames = new[] { "in_vertex" },
                UniformBufferNames = new[] { "ColorUb" },
            });
            texShaderProgram = glContext.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] { vs },
                FragmentShaders = new[] { texFs },
                VertexAttributeNames = new[] { "in_vertex" },
                SamplerNames = new[] { "Texture" }
            });

            var vertices = new[]
            {
                new Vector4(-1, -1, 0, 0),
                new Vector4(-1, 1, 0, 1),
                new Vector4(1, -1, 1, 0),
                new Vector4(1, 1, 1, 1),
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
            glContext.Bindings.Program.Set(colShaderProgram);
            glContext.Bindings.VertexArray.Set(vao);

            glContext.States.DepthStencil.DepthTestEnable.Set(false);
            glContext.States.Rasterizer.CullFaceEnable.Set(false);

            glContext.States.Blend.BlendEnable.Set(true);
            glContext.States.Blend.United.Equation.Set(BlendMode.Add);
            glContext.States.Blend.United.Function.Set(BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);

            colorUb.SetData(color4);
            colorUb.Bind(glContext, 0);

            glContext.Actions.Draw.Arrays(BeginMode.TriangleStrip, 0, 4);

            glContext.States.Blend.BlendEnable.Set(false);
            glContext.States.DepthStencil.DepthTestEnable.Set(true);
        }

        public void Draw(ITexture2D glTexture)
        {
            glContext.Bindings.Program.Set(texShaderProgram);
            glContext.Bindings.VertexArray.Set(vao);

            glContext.States.DepthStencil.DepthTestEnable.Set(false);
            glContext.States.Rasterizer.CullFaceEnable.Set(false);

            glContext.States.Blend.BlendEnable.Set(true);
            glContext.States.Blend.United.Equation.Set(BlendMode.Add);
            glContext.States.Blend.United.Function.Set(BlendFactor.SrcAlpha, BlendFactor.OneMinusSrcAlpha);

            glContext.Bindings.Textures.Units[0].Set(glTexture);

            glContext.Actions.Draw.Arrays(BeginMode.TriangleStrip, 0, 4);

            glContext.States.Blend.BlendEnable.Set(false);
            glContext.States.DepthStencil.DepthTestEnable.Set(true);
        }
    }
}