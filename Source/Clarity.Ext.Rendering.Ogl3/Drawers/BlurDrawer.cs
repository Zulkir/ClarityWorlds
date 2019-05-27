using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Ext.Rendering.Ogl3.Sugar;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Resources.Images;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;

namespace Clarity.Ext.Rendering.Ogl3.Drawers 
{
    public class BlurDrawer : IBlurDrawer
    {
        private readonly IContext glContext;
        private readonly IShaderProgram shaderProgram;
        private readonly IVertexArray vao;
        private readonly UniformBufferSugar<IntVector4> paramsUb;

        public unsafe BlurDrawer(IContext glContext)
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
    int OffsetX;
    int OffsetY;
    int Width;
    int Height;
};

in vec2 v_tex_coord;

out vec4 out_color;

void main()
{
    ivec2 coord = ivec2(gl_FragCoord);
    //vec4 color = vec4(0, 0, 0, 0);
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(-1, -1), 2) / 16;
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(-1, 0), 2) / 8;
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(-1, 1), 2) / 16;
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(0, -1), 2) / 8;
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(0, 0), 2) / 4;
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(0, 1), 2) / 8;
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(1, -1), 2) / 16;
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(1, 0), 2) / 8;
    //color += texelFetch(DiffuseMap, coord / 4 + ivec2(1, 1), 2) / 16;
    //out_color = color;
    //out_color = vec4(1, 0, 1, 1);
    //out_color = texture(DiffuseMap, vec2(gl_FragCoord.x / 500, gl_FragCoord.y / 500));// vec4(gl_FragCoord.x / 500, gl_FragCoord.y / 500, 0, 1);
    out_color = texture(DiffuseMap, vec2((gl_FragCoord.x + OffsetX) / Width, (gl_FragCoord.y + OffsetY) / Height), 4);
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

            paramsUb = new UniformBufferSugar<IntVector4>(glContext);
        }

        public void Draw(ITexture2D resolvedTexture)
        {
            var prevShader = glContext.Bindings.Program.Get();
            var prevVao = glContext.Bindings.VertexArray.Get();
            var prevUb = glContext.Bindings.Buffers.UniformIndexed[0].Get();
            var prevDepthMask = glContext.States.DepthStencil.DepthMask.Get();
            var prevDepthTestEnable = glContext.States.DepthStencil.DepthTestEnable.Get();
            var prevCullFaceEnable = glContext.States.Rasterizer.CullFaceEnable.Get();

            glContext.Bindings.Program.Set(shaderProgram);
            glContext.Bindings.VertexArray.Set(vao);
            
            glContext.States.DepthStencil.DepthMask.Set(false);
            glContext.States.DepthStencil.DepthTestEnable.Set(false);
            glContext.States.Rasterizer.CullFaceEnable.Set(false);

            glContext.Bindings.Textures.ActiveUnit.Set(0);
            glContext.Bindings.Textures.Units[0].Set(resolvedTexture);

            paramsUb.SetData(new IntVector4(0, 0, resolvedTexture.Width, resolvedTexture.Height));
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