using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Ext.Rendering.Ogl3.Sugar;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Resources.Images;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;

namespace Clarity.Ext.Rendering.Ogl3.Drawers 
{
    public class BleedDrawer : IBleedDrawer 
    {
        private readonly IContext glContext;
        private readonly IShaderProgram program;
        private readonly IVertexArray vao;
        private readonly UniformBufferSugar<Color4> paramsUb;

        public unsafe BleedDrawer(IContext glContext)
        {
            this.glContext = glContext;

            #region Shader Text
            const string vertexShaderText =
                @"#version 150

in vec2 in_position;
in vec2 in_tex_coord;

out vec2 v_tex_coord;

void main()
{
    gl_Position = vec4(in_position, 0, 1);
    v_tex_coord = in_tex_coord;
}
";

            const string fragmentShaderText =
                @"#version 150
uniform sampler2D DiffuseMap;

layout(std140) uniform Params
{
    vec4 Color;
};

in vec2 v_tex_coord;

out vec4 out_color;

void main()
{
    ivec2 coord = ivec2(gl_FragCoord);
    
    float accumulator = 0;
    for (int i = -4; i <= 4; i++)
    for (int j = -4; j <= 4; j++)
    {
        vec4 col = texelFetch(DiffuseMap, coord + ivec2(i, j), 0);
        accumulator += col.r + col.g + col.b;
    }

    float s = step(0.1, accumulator);
    out_color = s * Color;
}
";
            #endregion

            var vs = glContext.Create.VertexShader(vertexShaderText);
            var fs = glContext.Create.FragmentShader(fragmentShaderText);
            program = glContext.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] { vs },
                FragmentShaders = new[] { fs },
                VertexAttributeNames = new[] { "in_position" },
                UniformBufferNames = new [] { "Params" },
                SamplerNames = new [] { "DiffuseMap" },
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
            vao.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            //vao.SetVertexAttributeF(1, vertexBuffer, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            paramsUb = new UniformBufferSugar<Color4>(glContext);
        }

        public void Draw(ITexture2D resolvedTexture, Color4 color)
        {
            var prevShader = glContext.Bindings.Program.Get();
            var prevVao = glContext.Bindings.VertexArray.Get();
            var prevUb = glContext.Bindings.Buffers.UniformIndexed[0].Get();
            var prevDepthMask = glContext.States.DepthStencil.DepthMask.Get();
            var prevDepthTestEnable = glContext.States.DepthStencil.DepthTestEnable.Get();
            var prevCullFaceEnable = glContext.States.Rasterizer.CullFaceEnable.Get();

            glContext.Bindings.Program.Set(program);
            glContext.Bindings.VertexArray.Set(vao);
            
            glContext.States.DepthStencil.DepthMask.Set(false);
            glContext.States.DepthStencil.DepthTestEnable.Set(false);
            glContext.States.Rasterizer.CullFaceEnable.Set(false);

            glContext.Bindings.Textures.ActiveUnit.Set(0);
            glContext.Bindings.Textures.Units[0].Set(resolvedTexture);

            paramsUb.SetData(color);
            paramsUb.Bind(glContext, 0);

            glContext.Actions.Draw.Arrays(BeginMode.TriangleStrip, 0, 4);

            glContext.States.DepthStencil.DepthMask.Set(prevDepthMask);
            glContext.States.DepthStencil.DepthTestEnable.Set(prevDepthTestEnable);
            glContext.States.Rasterizer.CullFaceEnable.Set(prevCullFaceEnable);

            glContext.Bindings.Program.Set(prevShader);
            glContext.Bindings.VertexArray.Set(prevVao);
            glContext.Bindings.Buffers.UniformIndexed[0].Set(prevUb);
        }
    }
}