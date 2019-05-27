using System;
using Clarity.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.RawData;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Flexible
{
    // todo: refactor to no pack
    public static unsafe class FlexibleModelHelpers
    {
        // todo: remove source param
        public static IFlexibleModel CreateSimple(IResourceSource source, VertexPosNormTex[] vertices, int[] indices, FlexibleModelPrimitiveTopology primitiveTopology)
        {
            var pack = new ResourcePack(ResourceVolatility.Immutable);
            //pack.Source = source;

            RawDataResource vertexDataRes;
            fixed (VertexPosNormTex* pVertices = vertices)
                vertexDataRes = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pVertices, vertices.Length * sizeof(VertexPosNormTex));
            pack.AddSubresource("VertexArray", vertexDataRes);

            var hasIndices = indices != null;
            
            RawDataResource indexDataRes = null;
            if (hasIndices)
            {
                fixed (int* pIndices = indices)
                    indexDataRes = new RawDataResource(ResourceVolatility.Immutable, (IntPtr)pIndices, indices.Length * sizeof(int));
                pack.AddSubresource("IndexArray", indexDataRes);
            }

            var arraySubranges = hasIndices
                ? new[]
                {
                    vertexDataRes.GetSubrange(0),
                    indexDataRes.GetSubrange(0)
                }
                : new[]
                {
                    vertexDataRes.GetSubrange(0)
                };

            var elementInfos = VertexPosNormTex.GetElementsInfos(0);
            var indicesInfo = hasIndices ? new VertexIndicesInfo(1, CommonFormat.R32_UINT) : null;

            var vertexSet = new FlexibleModelVertexSet(ResourceVolatility.Immutable, arraySubranges, elementInfos, indicesInfo);
            pack.AddSubresource("VertexSet", vertexSet);

            var modelPart = new FlexibleModelPart
            {
                ModelMaterialName = "MainMaterial",
                VertexSetIndex = 0,
                PrimitiveTopology = primitiveTopology,
                IndexCount = hasIndices ? indices.Length : vertices.Length,
                FirstIndex = 0,
                VertexOffset = 0
            };

            var sphere = Sphere.BoundingSphere(vertices, x => x.Position);
            var model = new FlexibleModel(ResourceVolatility.Immutable, new[] { vertexSet }, new[] { modelPart }, sphere);
            pack.AddSubresource("Model", model);
            model.Source = source;

            return model;
        }
    }
}