using System.Linq;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Special.Sketching;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.Shaders;
using ObjectGL.Api.Objects.VertexArrays;
using PtrMagic;

namespace Clarity.Ext.Rendering.Ogl3
{
    public unsafe class SketchDrawer : ISketchDrawer
    {
        private readonly IContext glContext;
        private readonly ISketchService sketchService;
        private readonly IShaderProgram shaderProgram;
        private readonly IVertexArray vao;
        private Vector2[] data;
        private IBuffer vertexBuffer;

        public SketchDrawer(IContext glContext, ISketchService sketchService)
        {
            this.glContext = glContext;
            this.sketchService = sketchService;

            #region Shader Text
            const string vertexShaderText =
@"#version 150
in vec2 in_position;

void main()
{
    gl_Position = vec4(in_position, 0.0, 1.0f);
}
";

            const string fragmentShaderText =
@"#version 150
uniform samplerCube Texture;

out vec4 out_color;

void main()
{
    out_color = vec4(1.0, 0.0, 1.0, 1.0);
}
";
            #endregion

            var vs = glContext.Create.VertexShader(vertexShaderText);
            var fs = glContext.Create.FragmentShader(fragmentShaderText);
            shaderProgram = glContext.Create.Program(new ShaderProgramDescription
            {
                VertexShaders = new[] { vs },
                FragmentShaders = new[] { fs },
                VertexAttributeNames = new[] { "in_position" }
            });

            vao = glContext.Create.VertexArray();
        }

        public void Draw()
        {
            var sketches = sketchService.GetSketches();
            var totalPointCount = sketches.Sum(x => x.Count);

            if (totalPointCount == 0)
                return;

            if (data == null || totalPointCount > data.Length)
            {
                vertexBuffer?.Dispose();
                var size = data?.Length ?? 1024;
                while (size < totalPointCount)
                    size *= 2;

                data = new Vector2[size];
                vertexBuffer = glContext.Create.Buffer(BufferTarget.Array, data.Length * sizeof(Vector2), BufferUsageHint.DynamicDraw);
                vao.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, sizeof(Vector2), 0);
            }
            
            int offset = 0;
            foreach (var sketch in sketches)
            foreach (var point in sketch)
                data[offset++] = point;
            
            var sketchSize = totalPointCount * sizeof(Vector2);
            var map = vertexBuffer.Map(0, sketchSize, MapAccess.Write | MapAccess.InvalidateRange);
            fixed (Vector2* pData = data)
                PtrHelper.CopyBulk((byte*)map, (byte*)pData, sketchSize);
            vertexBuffer.Unmap();

            glContext.Bindings.Program.Set(shaderProgram);
            glContext.Bindings.VertexArray.Set(vao);

            glContext.States.DepthStencil.DepthTestEnable.Set(false);
            glContext.States.DepthStencil.DepthMask.Set(false);

            OpenTK.Graphics.OpenGL4.GL.LineWidth(6f);

            offset = 0;
            foreach (var sketch in sketches)
            {
                glContext.Actions.Draw.Arrays(BeginMode.LineStrip, offset, sketch.Count);
                offset += sketch.Count;
            }

            OpenTK.Graphics.OpenGL4.GL.LineWidth(1f);

            glContext.States.DepthStencil.DepthTestEnable.Set(true);
            glContext.States.DepthStencil.DepthMask.Set(true);
        }
    }
}