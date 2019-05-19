using System.Collections.Generic;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Numericals;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Flexible.Embedded
{
    public class SphereModelFactory : ResourceFactoryBase
    {
        public const string FactoryName = "Core_SphereModelFactory";
        public override string UniqueName => FactoryName;
        
        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, SphereModelFactory>
        {
            [TrwSerialize]
            public int HalfNumCircleSegments { get; set; }

            [TrwSerialize]
            public bool Inverse { get; set; }

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, SphereModelFactory factory) : base(factoryResourceCache, factory)
            {
            }

            protected override int GetFieldsHashCode() => 
                Inverse ? HalfNumCircleSegments : HalfNumCircleSegments << 1;

            protected override bool FieldsAreEual(Source other) =>
                HalfNumCircleSegments == other.HalfNumCircleSegments &&
                Inverse == other.Inverse;
        }

        public SphereModelFactory(IFactoryResourceCache factoryResourceCache) : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var cSource = (Source)resourceSource;
            var verticalIndexOffset = 2 * cSource.HalfNumCircleSegments;
            var vertices = new CgVertexPosNormTex[verticalIndexOffset * cSource.HalfNumCircleSegments];
            for (int i = 0; i < cSource.HalfNumCircleSegments; i++)
            for (int j = 0; j < verticalIndexOffset; j++)
            {
                var u = (float)j / (verticalIndexOffset - 1);
                var v = (float)i / (cSource.HalfNumCircleSegments - 1);
                var phi = MathHelper.TwoPi * u + MathHelper.Pi;
                var psi = MathHelper.PiOver2 - MathHelper.Pi * v;
                var z = MathHelper.Cos(phi) * MathHelper.Cos(psi);
                var x = MathHelper.Sin(phi) * MathHelper.Cos(psi);
                var y = MathHelper.Sin(psi);
                vertices[i * verticalIndexOffset + j] = new CgVertexPosNormTex(x, y, z, x, y, z, u, v);
            }
            var indexList = new List<int>();
            for (int i = 0; i < cSource.HalfNumCircleSegments - 1; i++)
            for (int j = 0; j < verticalIndexOffset - 1; j++)
            {
                var topLeftIndex = i * verticalIndexOffset + j;
                indexList.Add(topLeftIndex);
                indexList.Add(topLeftIndex + 1);
                indexList.Add(topLeftIndex + 1 + verticalIndexOffset);
                indexList.Add(topLeftIndex);
                indexList.Add(topLeftIndex + 1 + verticalIndexOffset);
                indexList.Add(topLeftIndex + verticalIndexOffset);
            }
            var indices = indexList.ToArray();
            if (cSource.Inverse)
            {
                for (int i = 0; i < vertices.Length; i++)
                    vertices[i].Normal = -vertices[i].Normal;
                for (int i = 0; i < indices.Length; i += 3)
                    CodingHelper.Swap(ref indices[i + 1], ref indices[i + 2]);
            }
            return FlexibleModelHelpers.CreateSimple(cSource, vertices, indices, FlexibleModelPrimitiveTopology.TriangleList);
        }

        public IResourceSource GetModelSource(int halfNumCircleSegments, bool inverse)
        {
            return new Source(FactoryResourceCache, this)
            {
                HalfNumCircleSegments = halfNumCircleSegments,
                Inverse = inverse
            };
        }
    }
}