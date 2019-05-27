using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Explicit 
{
    public class ExplicitModel : ResourceBase, IExplicitModel 
    {
        public Vector3[] Positions { get; set; }
        public Vector3[] Normals { get; set; }
        public Vector3[] Tangents { get; set; }
        public Color4[] Colors { get; set; }
        public Vector4[] BoneWeights { get; set; }
        public IntVector4[] BoneIndices { get; set; }
        public Vector2[] TexCoords { get; set; }
        public Vector2[] TexCoordsSecondary { get; set; }
        public Vector3[] TexCoordsCube { get; set; }
        public Vector3[] TexCoords3D { get; set; }
        
        public Vector4[] AbstractFloats0 { get; set; }
        public Vector4[] AbstractFloats1 { get; set; }
        public Vector4[] AbstractFloats2 { get; set; }
        public Vector4[] AbstractFloats3 { get; set; }

        public IntVector4[] AbstractInts0 { get; set; }
        public IntVector4[] AbstractInts1 { get; set; }
        public IntVector4[] AbstractInts2 { get; set; }
        public IntVector4[] AbstractInts3 { get; set; }

        public int[] Indices { get; set; }

        public ExplicitModelIndexSubrange[] IndexSubranges { get; set; }
        public ExplicitModelPrimitiveTopology Topology { get; set; }

        public Sphere BoundingSphere { get; set; }
        public bool IndicesAreTrivial { get; set; }
        public int MinIndexSize { get; set; }

        public int PartCount => IndexSubranges.Length;

        public ExplicitModel(ResourceVolatility volatility) : base(volatility)
        {
        }

        public void RecalculateInfo()
        {
            BoundingSphere = Sphere.BoundingSphere(Positions);
            IndicesAreTrivial = Enumerable.Range(0, Indices.Length).All(x => Indices[x] == x);
            MinIndexSize = Positions.Length <= byte.MaxValue
                ? sizeof(byte)
                : Positions.Length <= ushort.MaxValue
                    ? sizeof(ushort)
                    : sizeof(int);
        }

        public void Validate()
        {
            // todo
        }

        public IEnumerable<Tuple3<int>> EnumerateTriangles()
        {
            switch (Topology)
            {
                case ExplicitModelPrimitiveTopology.PointList:
                case ExplicitModelPrimitiveTopology.LineList:
                case ExplicitModelPrimitiveTopology.LineStrip:
                    throw new InvalidOperationException("Trying to enumerate triangles of a non-triangle model.");
                case ExplicitModelPrimitiveTopology.TriangleList:
                    for (var i = 0; i < Indices.Length; i += 3)
                        yield return Tuples.SameTypeTuple(Indices[i], Indices[i + 1], Indices[i + 2]);
                    break;
                case ExplicitModelPrimitiveTopology.TriangleStrip:
                    var odd = 0;
                    for (var i = 0; i < Indices.Length - 2; i++)
                    {
                        yield return Tuples.SameTypeTuple(Indices[i], Indices[i + 1 + odd], Indices[i + 2 - odd]);
                        odd ^= 1;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IExplicitModel GenerateTangents()
        {
            throw new NotImplementedException();
            //if (Normals == null || TexCoords == null)
            //    throw new InvalidOperationException("Normals and TexCoords are required to generate Tangents.");
            //Tangents = new Vector3[Positions.Length];
            //for (var i = 0; i < Tangents.Length; i++)
            //{
            //    var numTriangles = 0;
            //    var tangentAggregator = Vector3.Zero;
            //    foreach (var triangle in EnumerateTriangles())
            //    {
            //        if (triangle.Item0 != i && triangle.Item1 != i && triangle.Item2 != i)
            //            continue;
            //        // todo
            //    }
            //}
        }

        public static IExplicitModel FromVertices(IReadOnlyList<VertexPos> vertices, IReadOnlyList<int> indices, ExplicitModelPrimitiveTopology topology)
        {
            var positions = vertices.Select(x => x.Position).ToArray();
            var model = new ExplicitModel(ResourceVolatility.Immutable)
            {
                Positions = positions,
                
                Indices = indices?.ToArray() ?? Enumerable.Range(0, positions.Length).ToArray(),
                IndexSubranges = new[] { new ExplicitModelIndexSubrange(0, indices?.Count ?? vertices.Count) },
                Topology = topology,
            };
            model.RecalculateInfo();
            model.Validate();
            return model;
        }

        public static IExplicitModel FromVertices(IReadOnlyList<VertexPosNormTex> vertices, IReadOnlyList<int> indices, ExplicitModelPrimitiveTopology topology)
        {
            var positions = vertices.Select(x => x.Position).ToArray();
            var model = new ExplicitModel(ResourceVolatility.Immutable)
            {
                Positions = positions,
                Normals = vertices.Select(x => x.Normal).ToArray(),
                TexCoords = vertices.Select(x => x.TexCoord).ToArray(),
                
                Indices = indices?.ToArray() ?? Enumerable.Range(0, positions.Length).ToArray(),
                IndexSubranges = new[] { new ExplicitModelIndexSubrange(0, indices?.Count ?? vertices.Count) },
                Topology = topology,
            };
            model.RecalculateInfo();
            model.Validate();
            return model;
        }

        public static IExplicitModel FromVertices(IReadOnlyList<VertexPosTanNormTex> vertices, IReadOnlyList<int> indices, ExplicitModelPrimitiveTopology topology)
        {
            var positions = vertices.Select(x => x.Position).ToArray();
            var model = new ExplicitModel(ResourceVolatility.Immutable)
            {
                Positions = positions,
                Tangents = vertices.Select(x => x.Tangent).ToArray(),
                Normals = vertices.Select(x => x.Normal).ToArray(),
                TexCoords = vertices.Select(x => x.TexCoord).ToArray(),
                
                Indices = indices?.ToArray() ?? Enumerable.Range(0, positions.Length).ToArray(),
                IndexSubranges = new []{new ExplicitModelIndexSubrange(0, indices?.Count ?? vertices.Count)},
                Topology = topology,
            };
            model.RecalculateInfo();
            model.Validate();
            return model;
        }
    }
}