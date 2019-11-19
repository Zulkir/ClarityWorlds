using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Explicit.Embedded
{
    public enum PlaneModelSourcePlane
    {
        Xy,
        Xz
    }

    public enum PlaneModelSourceNormalDirection
    {
        Positive,
        Negative
    }

    public class PlaneModelFactory : ResourceFactoryBase
    {
        public override string UniqueName => "Core_PlaneModelFactory";
        
        private class Source : FactoryResourceSourceBase<Source, PlaneModelFactory>
        {
            [TrwSerialize]
            public PlaneModelSourcePlane Plane { get; set; }

            [TrwSerialize]
            public PlaneModelSourceNormalDirection NormalDirection { get; set; }

            [TrwSerialize]
            public float HalfWidth { get; set; }

            [TrwSerialize]
            public float HalfHeight { get; set; }

            [TrwSerialize]
            public float ScaleU { get; set; }

            [TrwSerialize]
            public float ScaleV { get; set; }

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, PlaneModelFactory factory) : base(factoryResourceCache, factory)
            {
            }

            protected override bool FieldsAreEqual(Source other) =>
                Plane == other.Plane &&
                NormalDirection == other.NormalDirection &&
                HalfWidth == other.HalfWidth &&
                HalfHeight == other.HalfHeight &&
                ScaleU == other.ScaleU &&
                ScaleV == other.ScaleV;

            protected override int GetFieldsHashCode() =>
                (int)Plane |
                ((int)NormalDirection << 1) |
                (HalfWidth.GetHashCode() << 2) ^
                (HalfHeight.GetHashCode() << 3) ^
                (ScaleU.GetHashCode() << 4) ^
                (ScaleV.GetHashCode() << 5);
        }

        public PlaneModelFactory(IFactoryResourceCache factoryResourceCache) : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var cSource = (Source)resourceSource;
            var ns = cSource.NormalDirection == PlaneModelSourceNormalDirection.Positive ? 1f : -1f;
            var hw = cSource.HalfWidth;
            var hh = cSource.HalfHeight;
            var su = cSource.ScaleU;
            var sv = cSource.ScaleV;
            var vertices = cSource.Plane == PlaneModelSourcePlane.Xy 
                ? new[]
                {
                    new VertexPosTanNormTex(new Vector3(-hw, -hh, 0), new Vector3(1, 0, 0), new Vector3(0, 0, ns), new Vector2(0, hh * sv)),
                    new VertexPosTanNormTex(new Vector3(-hw,  hh, 0), new Vector3(1, 0, 0), new Vector3(0, 0, ns), new Vector2(0, 0)),
                    new VertexPosTanNormTex(new Vector3( hw,  hh, 0), new Vector3(1, 0, 0), new Vector3(0, 0, ns), new Vector2(hw * su, 0)),
                    new VertexPosTanNormTex(new Vector3( hw, -hh, 0), new Vector3(1, 0, 0), new Vector3(0, 0, ns), new Vector2(hw * su, hh * sv)),
                } 
                : new[]
                {
                    new VertexPosTanNormTex(new Vector3(-hw, 0,  hh), new Vector3(1, 0, 0), new Vector3(0, ns, 0), new Vector2(0, hh * sv)),
                    new VertexPosTanNormTex(new Vector3(-hw, 0, -hh), new Vector3(1, 0, 0), new Vector3(0, ns, 0), new Vector2(0, 0)),
                    new VertexPosTanNormTex(new Vector3( hw, 0, -hh), new Vector3(1, 0, 0), new Vector3(0, ns, 0), new Vector2(hw * su, 0)),
                    new VertexPosTanNormTex(new Vector3( hw, 0,  hh), new Vector3(1, 0, 0), new Vector3(0, ns, 0), new Vector2(hw * su, hh * sv)),
                };
            var indices = cSource.NormalDirection == PlaneModelSourceNormalDirection.Positive
                ? new[] { 0, 1, 2, 0, 2, 3 }
                : new[] { 0, 2, 1, 0, 3, 2 };
            return ExplicitModel.FromVertices(vertices, indices, ExplicitModelPrimitiveTopology.TriangleList).WithSource(cSource);
        }

        public IResourceSource GetModelSource(PlaneModelSourcePlane plane, PlaneModelSourceNormalDirection normalDirection, 
                                                     float halfWidth, float halfHeight, float scaleU, float scaleV)
        {
            return new Source(FactoryResourceCache, this)
            {
                HalfWidth = halfWidth,
                HalfHeight = halfHeight,
                ScaleU = scaleU,
                ScaleV = scaleV,
                Plane = plane,
                NormalDirection = normalDirection
            };
        }
    }
}