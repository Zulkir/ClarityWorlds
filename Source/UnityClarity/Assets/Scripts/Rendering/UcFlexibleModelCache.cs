using System;
using System.Linq;
using Clarity.Common;
using Clarity.Common.GraphicalGeometry;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.Caching;
using UnityEngine;

namespace Assets.Scripts.Rendering
{
    public unsafe class UcFlexibleModelCache : ICache
    {
        private readonly IFlexibleModel cgModel;
        private Mesh unityMesh;
        private bool dirty;

        public UcFlexibleModelCache(IFlexibleModel cgModel)
        {
            this.cgModel = cgModel;
            dirty = true;
        }

        public void Dispose()
        {
            // todo: main thread disposer
        }

        public void OnMasterEvent(object eventArgs)
        {
            dirty = true;
        }

        public Mesh GetUnityMesh()
        {
            if (!dirty)
                return unityMesh;

            unityMesh = new Mesh();
            
            // todo: correctly process multi-part models
            var part = cgModel.Parts[0];
            var vertexSet = cgModel.VertexSets[part.VertexSetIndex];
            var indices = ExtractIndices(vertexSet, part);
            var maxIndex = indices.Max();
            var positions = ExtractVertexElements(vertexSet, maxIndex, CommonVertexSemantic.Position, CommonFormat.R32G32B32_SFLOAT, x => ConvertPosAndNormal(*(Vector3*)x));
            var normals = ExtractVertexElements(vertexSet, maxIndex, CommonVertexSemantic.Normal, CommonFormat.R32G32B32_SFLOAT, x => ConvertPosAndNormal(*(Vector3*)x));
            var texcoords = ExtractVertexElements(vertexSet, maxIndex, CommonVertexSemantic.TexCoord, CommonFormat.R32G32_SFLOAT, x => ConvertTexCoord(*(Vector2*)x));
            unityMesh.vertices = positions;
            unityMesh.normals = normals;
            unityMesh.uv = texcoords;
            //unityMesh.triangles = indices;
            unityMesh.SetIndices(indices, TopologyToUnity(part.PrimitiveTopology), 0);

            dirty = false;

            return unityMesh;
        }

        private static T[] ExtractVertexElements<T>(IFlexibleModelVertexSet vertexSet, int maxIndex, CommonVertexSemantic semantic, CommonFormat expectedFormat, Func<IntPtr, T> decode)
        {
            var elementInfo = vertexSet.ElementInfos.FirstOrDefault(x => x.CommonSemantic == semantic);
            if (elementInfo == null)
                return null;
            
            var vertexCount = maxIndex + 1;
            var elements = new T[vertexCount];

            var arraySubrange = vertexSet.ArraySubranges[elementInfo.ArrayIndex];
            var pCgVertices = arraySubrange.RawDataResource.Map() + arraySubrange.StartOffset;
            var pCurrentElem = pCgVertices + elementInfo.Offset;
            if (elementInfo.Format != expectedFormat)
                throw new NotImplementedException($"Only '{expectedFormat}' format is supported for '{typeof(T).Name}' elems at the moment.");
            for (int i = 0; i < vertexCount; i++)
            {
                elements[i] = decode(pCurrentElem);
                pCurrentElem += elementInfo.Stride;
            }
            arraySubrange.RawDataResource.Unmap(false);
            return elements;
        }

        private static int[] ExtractIndices(IFlexibleModelVertexSet vertexSet, IFlexibleModelPart part)
        {
            var indexCount = part.IndexCount;
            var indices = new int[indexCount];
            if (vertexSet.IndicesInfo == null)
            {
                for (int i = 0; i < indexCount; i++)
                    indices[i] = i + part.FirstIndex + part.VertexOffset;
            }
            else
            {
                var indexArraySubrange = vertexSet.ArraySubranges[vertexSet.IndicesInfo.ArrayIndex];
                var pCgIndices = indexArraySubrange.RawDataResource.Map() + indexArraySubrange.StartOffset;
                switch (vertexSet.IndicesInfo.Format)
                {
                    case CommonFormat.R8_UINT:
                        var pCgIndices8 = (byte*)pCgIndices + part.FirstIndex;
                        for (int i = 0; i < indexCount; i++)
                            indices[i] = pCgIndices8[i] + part.VertexOffset;
                        break;
                    case CommonFormat.R16_UINT:
                        var pCgIndices16 = (ushort*)pCgIndices + part.FirstIndex;
                        for (int i = 0; i < indexCount; i++)
                            indices[i] = pCgIndices16[i] + part.VertexOffset;
                        break;
                    case CommonFormat.R32_UINT:
                        var pCgIndices32 = (int*)pCgIndices + part.FirstIndex;
                        for (int i = 0; i < indexCount; i++)
                            indices[i] = pCgIndices32[i] + part.VertexOffset;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                indexArraySubrange.RawDataResource.Unmap(false);
            }
            return indices;
        }

        private MeshTopology TopologyToUnity(FlexibleModelPrimitiveTopology cgTopology)
        {
            switch (cgTopology)
            {
                case FlexibleModelPrimitiveTopology.PointList: return MeshTopology.Points;
                case FlexibleModelPrimitiveTopology.LineList: return MeshTopology.Lines;
                case FlexibleModelPrimitiveTopology.LineStrip: return MeshTopology.LineStrip;
                case FlexibleModelPrimitiveTopology.TriangleList: return MeshTopology.Triangles;
                case FlexibleModelPrimitiveTopology.TriangleStrip:
                case FlexibleModelPrimitiveTopology.TriangleFan:
                case FlexibleModelPrimitiveTopology.LineListWithAdjacency:
                case FlexibleModelPrimitiveTopology.LineStripWithAdjacency:
                case FlexibleModelPrimitiveTopology.TriangleListWithAdjacency:
                case FlexibleModelPrimitiveTopology.TriangleStripWithAdjacency:
                case FlexibleModelPrimitiveTopology.PatchList:
                default:
                    throw new NotImplementedException($"Topology '{cgTopology}' is not yet supported by the Unity backend of Clarity Worlds.");
            }
        }

        private static Vector3 ConvertPosAndNormal(Vector3 v)
        {
            v.z = -v.z;
            return v;
        }

        private static Vector2 ConvertTexCoord(Vector2 v)
        {
            v.y = 1f - v.y;
            return v;
        }
    }
}