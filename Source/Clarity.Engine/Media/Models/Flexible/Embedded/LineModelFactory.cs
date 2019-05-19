using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Flexible.Embedded
{
    public class LineModelFactory : ResourceFactoryBase
    {
        public override string UniqueName => "Core_LineModelFactory";

        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, LineModelFactory>
        {
            protected override int GetFieldsHashCode() => 0;
            protected override bool FieldsAreEual(Source other) => true;

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, LineModelFactory factory) : base(factoryResourceCache, factory)
            {
            }
        }

        public LineModelFactory(IFactoryResourceCache factoryResourceCache) : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var vertices = new[]
            {
                new CgVertexPosNormTex(0, 0, 0, 0, 0, 0, 0, 0),
                new CgVertexPosNormTex(1, 0, 0, 0, 0, 0, 1, 0),
            };

            return FlexibleModelHelpers.CreateSimple(resourceSource, vertices, null, FlexibleModelPrimitiveTopology.LineList);
        }

        public IResourceSource GetModelSource()
        {
            return new Source(FactoryResourceCache, this);
        }
    }
}