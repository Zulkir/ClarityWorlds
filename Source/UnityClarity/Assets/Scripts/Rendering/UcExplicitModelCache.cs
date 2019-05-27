using System;
using System.Linq;
using Assets.Scripts.Helpers;
using Clarity.Engine.Media.Models.Explicit;
using Clarity.Engine.Objects.Caching;
using UnityEngine;

namespace Assets.Scripts.Rendering
{
    public class UcExplicitModelCache : ICache
    {
        private readonly IExplicitModel cgModel;
        private Mesh unityMesh;
        private bool dirty;

        public UcExplicitModelCache(IExplicitModel cgModel)
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

            unityMesh = new Mesh
            {
                subMeshCount = cgModel.IndexSubranges.Length,
                vertices = cgModel.Positions?.Select(x => x.ToUnity()).ToArray(),
                normals = cgModel.Normals?.Select(x => x.ToUnity()).ToArray(),
                tangents = cgModel.Tangents?.Select(x => x.ToUnity4()).ToArray(),
                uv = cgModel.TexCoords?.Select(x => x.ToUnity(true)).ToArray()
            };

            for (var partIndex = 0; partIndex < cgModel.IndexSubranges.Length; partIndex++)
            {
                var indexSubrange = cgModel.IndexSubranges[partIndex];
                var indices = new int[indexSubrange.IndexCount];
                Array.Copy(cgModel.Indices, indexSubrange.FirstIndex, indices, 0, indexSubrange.IndexCount);
                unityMesh.SetIndices(indices, TopologyToUnity(cgModel.Topology), partIndex);
            }

            dirty = false;
            return unityMesh;
        }

        private static MeshTopology TopologyToUnity(ExplicitModelPrimitiveTopology cgTopology)
        {
            switch (cgTopology)
            {
                case ExplicitModelPrimitiveTopology.PointList: return MeshTopology.Points;
                case ExplicitModelPrimitiveTopology.LineList: return MeshTopology.Lines;
                case ExplicitModelPrimitiveTopology.LineStrip: return MeshTopology.LineStrip;
                case ExplicitModelPrimitiveTopology.TriangleList: return MeshTopology.Triangles;
                default: throw new NotImplementedException($"Topology '{cgTopology}' is not yet supported by the Unity backend of Clarity Worlds.");
            }
        }
    }
}