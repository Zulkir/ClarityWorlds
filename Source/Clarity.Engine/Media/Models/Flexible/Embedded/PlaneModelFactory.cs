using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Flexible.Embedded
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

            protected override bool FieldsAreEual(Source other) =>
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
            var normalSign = cSource.NormalDirection == PlaneModelSourceNormalDirection.Positive ? 1f : -1f;
            var vertices = cSource.Plane == PlaneModelSourcePlane.Xy 
                ? new[]
                {
                    new CgVertexPosNormTex(-cSource.HalfWidth, -cSource.HalfHeight, 0f, 0f, 0f, normalSign, 0f, cSource.HalfHeight * cSource.ScaleV),
                    new CgVertexPosNormTex(-cSource.HalfWidth, cSource.HalfHeight, 0f, 0f, 0f, normalSign, 0f, 0f),
                    new CgVertexPosNormTex(cSource.HalfWidth, cSource.HalfHeight, 0f, 0f, 0f, normalSign, cSource.HalfWidth * cSource.ScaleU, 0f),
                    new CgVertexPosNormTex(cSource.HalfWidth, -cSource.HalfHeight, 0f, 0f, 0f, normalSign, cSource.HalfWidth * cSource.ScaleU, cSource.HalfHeight * cSource.ScaleV),
                } 
                : new[]
                {
                    new CgVertexPosNormTex(-cSource.HalfWidth, 0f, cSource.HalfHeight, 0f, normalSign, 0f, 0f, cSource.HalfHeight * cSource.ScaleV),
                    new CgVertexPosNormTex(-cSource.HalfWidth, 0f, -cSource.HalfHeight, 0f, normalSign, 0f, 0f, 0f),
                    new CgVertexPosNormTex(cSource.HalfWidth, 0f, -cSource.HalfHeight, 0f, normalSign, 0f, cSource.HalfWidth * cSource.ScaleU, 0f),
                    new CgVertexPosNormTex(cSource.HalfWidth, 0f, cSource.HalfHeight, 0f, normalSign, 0f, cSource.HalfWidth * cSource.ScaleU, cSource.HalfHeight * cSource.ScaleV),
                };
            var indices = cSource.NormalDirection == PlaneModelSourceNormalDirection.Positive
                ? new[] { 0, 1, 2, 0, 2, 3 }
                : new[] { 0, 2, 1, 0, 3, 2 };
            return FlexibleModelHelpers.CreateSimple(cSource, vertices, indices, FlexibleModelPrimitiveTopology.TriangleList);
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