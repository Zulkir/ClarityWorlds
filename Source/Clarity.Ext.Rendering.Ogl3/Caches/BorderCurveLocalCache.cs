using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using ObjectGL.Api.Context;
using ObjectGL.Api.Objects.Resources.Buffers;
using ObjectGL.Api.Objects.VertexArrays;
using PtrMagic;

namespace Clarity.Ext.Rendering.Ogl3.Caches
{
    public class BorderCurveLocalCache : IBorderCurveLocalCache
    {
        private readonly IContext glContext;
        private readonly ISceneNode entity;
        private readonly IRichTextComponent aspect;
        private IBuffer vertexBuffer;
        private int bufferPointCapacity;
        private IVertexArray vao;
        private int lastPointCount;
        private bool isDirty;

        public BorderCurveLocalCache(IContext glContext, ISceneNode entity)
        {
            this.glContext = glContext;
            this.entity = entity;
            aspect = entity.GetComponent<IRichTextComponent>();
            bufferPointCapacity = 512;
            isDirty = true;
        }

        public void Dispose()
        {
            vao?.Dispose();
        }

        public void OnMasterEvent(object args)
        {
            isDirty = true;
        }

        public int NumPoints =>
            aspect.BorderComplete 
                ? aspect.VisualBorderCurve?.Count + 1 ?? 0 
                : aspect.VisualBorderCurve?.Count ?? 0;

        public unsafe IVertexArray GetVao()
        {
            if (!isDirty && NumPoints == lastPointCount)
                return vao;

            if (aspect.VisualBorderCurve == null || aspect.VisualBorderCurve.Count == 0)
                return null;

            if (vao == null)
                vao = glContext.Create.VertexArray();

            var newPointCount = NumPoints;
            if (vertexBuffer == null || bufferPointCapacity < newPointCount)
            {
                vertexBuffer?.Dispose();
                while (bufferPointCapacity < newPointCount)
                    bufferPointCapacity *= 2;
                vertexBuffer = glContext.Create.Buffer(BufferTarget.Array, bufferPointCapacity * sizeof(Vector4), BufferUsageHint.DynamicDraw);
                vao.SetVertexAttributeF(0, vertexBuffer, VertexAttributeDimension.Four, VertexAttribPointerType.Float, false, sizeof(Vector4), 0);
            }
            
            var rawData = aspect.BorderComplete
                ? aspect.VisualBorderCurve.Concat(aspect.VisualBorderCurve.First().EnumSelf()).Select(xy => new Vector4(xy, 0, 1)).ToArray()
                : aspect.VisualBorderCurve.Select(xy => new Vector4(xy, 0, 1)).ToArray();

            var sizeInBytes = rawData.Length * sizeof(Vector4);
            var dst = vertexBuffer.Map(0, vertexBuffer.SizeInBytes, MapAccess.Write | MapAccess.InvalidateBuffer);
            fixed (Vector4* src = rawData)
                PtrHelper.CopyBulk((byte*)dst, (byte*)src, sizeInBytes);
            vertexBuffer.Unmap();

            lastPointCount = newPointCount;
            isDirty = false;
            return vao;
        }
    }
}