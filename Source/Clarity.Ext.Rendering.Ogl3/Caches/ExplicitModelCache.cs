using System;
using System.Linq;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Media.Models.Explicit;
using Clarity.Engine.Objects.Caching;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.VertexArrays;

namespace Clarity.Ext.Rendering.Ogl3.Caches
{
    public class ExplicitModelCache : ICache
    {
        public struct GlObjects
        {
            public IVertexArray Vao;
            public IBuffer VertexBuffer;
            public IBuffer IndexBuffer;
            public bool SixteenBitIndices;
        }

        private readonly IGraphicsInfra infra;
        private readonly IExplicitModel model;
        private GlObjects glObjects;
        private bool dirty;

        public ExplicitModelCache(IGraphicsInfra infra, IExplicitModel model)
        {
            this.infra = infra;
            this.model = model;
            dirty = true;
        }

        public void Dispose()
        {
            infra.MainThreadDisposer.Add(glObjects.Vao);
            infra.MainThreadDisposer.Add(glObjects.VertexBuffer);
            infra.MainThreadDisposer.Add(glObjects.IndexBuffer);
        }

        public void OnMasterEvent(object eventArgs) => dirty = true;

        public unsafe GlObjects GetGlObjects()
        {
            if (!dirty)
                return glObjects;

            if (glObjects.Vao == null)
                glObjects.Vao = infra.GlContext.Create.VertexArray();

            glObjects.Vao.DisableVertexAttributesStartingFrom(0);

            var bufferUsageHint = model.Volatility == ResourceVolatility.Volatile
                ? BufferUsageHint.DynamicRead
                : BufferUsageHint.StaticRead;

            // todo: other formats
            var vertices = new VertexPosTanNormTex[model.Positions.Length];
            var getTangent = model.Tangents != null ? (Func<IExplicitModel, int, Vector3>)((m, i) => m.Tangents[i]) : (m, i) => Vector3.Zero;
            var getNormal = model.Normals != null ? (Func<IExplicitModel, int, Vector3>)((m, i) => m.Normals[i]) : (m, i) => Vector3.Zero;
            var getTexCoord = model.TexCoords != null ? (Func<IExplicitModel, int, Vector2>)((m, i) => m.TexCoords[i]) : (m, i) => Vector2.Zero;
            for (var i = 0; i < vertices.Length; i++)
                vertices[i] = new VertexPosTanNormTex(model.Positions[i], getTangent(model, i), getNormal(model, i), getTexCoord(model, i));
            var vertexBufferSize = vertices.Length * VertexPosTanNormTex.SizeInBytes;
            if (glObjects.VertexBuffer == null || glObjects.VertexBuffer.SizeInBytes != vertexBufferSize)
            {
                glObjects.VertexBuffer?.Dispose();
                glObjects.VertexBuffer = infra.GlContext.Create.Buffer(BufferTarget.Array, vertexBufferSize, bufferUsageHint);
            }

            fixed (VertexPosTanNormTex* pVertices = vertices)
                glObjects.VertexBuffer.SetData((IntPtr)pVertices);

            var elemInfos = VertexPosTanNormTex.GetElementsInfos(0);
            var positionInfo = elemInfos.Single(x => x.CommonSemantic == CommonVertexSemantic.Position);
            var normalInfo = elemInfos.Single(x => x.CommonSemantic == CommonVertexSemantic.Normal);
            var tangentInfo = elemInfos.Single(x => x.CommonSemantic == CommonVertexSemantic.Tangent);
            var texCoordInfo = elemInfos.Single(x => x.CommonSemantic == CommonVertexSemantic.TexCoord);

            glObjects.Vao.SetVertexAttributeF(0, glObjects.VertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, positionInfo.Stride, positionInfo.Offset);
            glObjects.Vao.SetVertexAttributeF(1, glObjects.VertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, normalInfo.Stride, normalInfo.Offset);
            glObjects.Vao.SetVertexAttributeF(2, glObjects.VertexBuffer, VertexAttributeDimension.Two, VertexAttribPointerType.Float, false, texCoordInfo.Stride, texCoordInfo.Offset);
            glObjects.Vao.SetVertexAttributeF(3, glObjects.VertexBuffer, VertexAttributeDimension.Three, VertexAttribPointerType.Float, false, tangentInfo.Stride, tangentInfo.Offset);

            if (!model.IndicesAreTrivial)
            {
                glObjects.SixteenBitIndices = model.MinIndexSize < sizeof(int);
                var indexSize = glObjects.SixteenBitIndices ? sizeof(ushort) : sizeof(int);
                var indexBufferSize = model.Indices.Length * indexSize;
                if (glObjects.IndexBuffer == null || glObjects.IndexBuffer.SizeInBytes != indexBufferSize)
                {
                    glObjects.Vao.SetElementArrayBuffer(null);
                    glObjects.IndexBuffer?.Dispose();
                    glObjects.IndexBuffer = infra.GlContext.Create.Buffer(BufferTarget.ElementArray, indexBufferSize, bufferUsageHint);
                }

                if (glObjects.SixteenBitIndices)
                {
                    var indices = model.Indices.Select(x => (ushort)x).ToArray();
                    fixed (ushort* pIndices = indices)
                        glObjects.IndexBuffer.SetData((IntPtr)pIndices);
                }
                else
                {
                    fixed (int* pIndices = model.Indices)
                        glObjects.IndexBuffer.SetData((IntPtr)pIndices);
                }

                glObjects.Vao.SetElementArrayBuffer(glObjects.IndexBuffer);
            }
            else
            {
                glObjects.Vao.SetElementArrayBuffer(null);
                glObjects.IndexBuffer?.Dispose();
                glObjects.IndexBuffer = null;
            }

            dirty = false;
            return glObjects;
        }
    }
}