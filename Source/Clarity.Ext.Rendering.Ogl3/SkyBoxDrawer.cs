using System;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Ext.Rendering.Ogl3.Sugar;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Samplers;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;
using OpenTK.Graphics.OpenGL4;
using BeginMode = ObjectGL.Api.Context.Actions.BeginMode;
using BufferTarget = ObjectGL.Api.Objects.Resources.Buffers.BufferTarget;
using BufferUsageHint = ObjectGL.Api.Objects.Resources.Buffers.BufferUsageHint;
using DrawElementsType = ObjectGL.Api.Context.Actions.DrawElementsType;
using TextureMagFilter = ObjectGL.Api.Objects.Samplers.TextureMagFilter;
using TextureMinFilter = ObjectGL.Api.Objects.Samplers.TextureMinFilter;
using TextureWrapMode = ObjectGL.Api.Objects.Samplers.TextureWrapMode;
using VertexAttribPointerType = ObjectGL.Api.Objects.VertexArrays.VertexAttribPointerType;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class SkyboxDrawer : ISkyboxDrawer
    {
        private readonly IContext glContext;
        private readonly ISampler sampler;
        private readonly IShaderProgram shaderProgram;
        private readonly IVertexArray vao;
        private readonly UniformBufferSugar<Matrix4x4> cameraUb;

        public unsafe SkyboxDrawer(IContext glContext)
        {
            this.glContext = glContext;
            sampler = glContext.Create.Sampler();
            sampler.SetMagFilter(TextureMagFilter.Linear);
            sampler.SetMinFilter(TextureMinFilter.LinearMipmapLinear);
            sampler.SetWrapR(TextureWrapMode.Clamp);
            sampler.SetWrapS(TextureWrapMode.Clamp);
            sampler.SetWrapT(TextureWrapMode.Clamp);
            sampler.SetMaxAnisotropy(16f);

            #region Shader Text
            const string vertexShaderText =
@"#version 150
layout(std140) uniform Camera
{
    mat4 ViewProjection;
};

in vec3 in_position;

out vec3 v_tex_coord;

void main()
{
    vec4 worldPosition = vec4(in_position, 1.0f);

    gl_Position = worldPosition * ViewProjection;
    gl_Position.z = 2.0 * gl_Position.z - gl_Position.w;

    v_tex_coord = vec3(-worldPosition.x, worldPosition.y, worldPosition.z);
}
";

            const string fragmentShaderText =
@"#version 150
uniform samplerCube Texture;

in vec3 v_tex_coord;

out vec4 out_color;

void main()
{
    out_color = texture(Texture, v_tex_coord);
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
                UniformBufferNames = new[] { "Camera" },
                SamplerNames = new[] { "Texture" }
            });
            
            var vertices = new[]
            {
                new Vector4(-10, -10, -1, 1), 
                new Vector4(-10, 10, -1, 1), 
                new Vector4(10, 10, -1, 1), 
                new Vector4(10, -10, -1, 1),

                new Vector4(-10, -10, 1, 1),
                new Vector4(-10, 10, 1, 1),
                new Vector4(10, 10, 1, 1),
                new Vector4(10, -10, 1, 1),
            };
            
            var indices = new[]
            {
                0, 1, 2, 0, 2, 3,
                4, 5, 6, 4, 6, 7,
                0, 4, 7, 0, 7, 3,
                0, 4, 5, 0, 5, 1,
                1, 5, 6, 1, 6, 2,
                3, 7, 6, 3, 6, 2
            };

            IBuffer vertexBuffer;
            fixed (Vector4* data = vertices)
                vertexBuffer = glContext.Create.Buffer(BufferTarget.Array, 4 * sizeof(float) * vertices.Length, BufferUsageHint.StaticDraw, (IntPtr)data);

            IBuffer indexBuffer;
            fixed (int* data = indices)
                indexBuffer = glContext.Create.Buffer(BufferTarget.ElementArray, sizeof(int) * indices.Length, BufferUsageHint.StaticDraw, (IntPtr)data);

            vao = glContext.Create.VertexArray();
            vao.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            vao.SetElementArrayBuffer(indexBuffer);

            cameraUb = new UniformBufferSugar<Matrix4x4>(glContext);
        }
        
        public void Draw(IDrawableTextureCube skyboxTexture, CameraFrame cameraFrame, float fieldOfView, float aspectRatio)
        {
            glContext.States.DepthStencil.DepthTestEnable.Set(false);
            glContext.States.DepthStencil.DepthMask.Set(false);
            glContext.GL.Enable((int)All.TextureCubeMapSeamless);

            glContext.Bindings.Program.Set(shaderProgram);
            glContext.Bindings.VertexArray.Set(vao);

            var simplifiedViewFrame = new CameraFrame(Vector3.Zero, cameraFrame.Forward, cameraFrame.Up, cameraFrame.Right);
            var viewMat = simplifiedViewFrame.GetViewMat();
            var projMat = Matrix4x4.PerspectiveFovDx(fieldOfView, aspectRatio, 0.1f, 100f);
            cameraUb.SetData(viewMat * projMat);
            cameraUb.Bind(glContext, 0);
            
            glContext.Bindings.Textures.ActiveUnit.Set(0);
            glContext.Bindings.Textures.Units[0].Set(skyboxTexture.GlTextureCube);
            glContext.Bindings.Samplers[0].Set(sampler);

            glContext.States.Rasterizer.CullFaceEnable.Set(false);

            glContext.Actions.Draw.Elements(BeginMode.Triangles, 36, DrawElementsType.UnsignedInt, 0);
        }
    }
}