using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Explicit.Embedded
{
    public class RectangleModelFactory : ResourceFactoryBase
    {
        public override string UniqueName => "RectangleModelFactory";

        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, RectangleModelFactory>
        {
            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, RectangleModelFactory factory)
                : base(factoryResourceCache, factory)
            {
            }

            protected override int GetFieldsHashCode() => 0;
            protected override bool FieldsAreEqual(Source other) => true;
        }

        public RectangleModelFactory(IFactoryResourceCache factoryResourceCache)
            : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var cSource = (Source)resourceSource;
            var vertices = new[]
            {
                new VertexPos(-1, -1, 0), 
                new VertexPos(-1,  1, 0), 
                new VertexPos( 1,  1, 0), 
                new VertexPos( 1, -1, 0), 
            };
            var indices = new[]
            {
                0, 1, 2, 3, 0
            };
            return ExplicitModel.FromVertices(vertices, indices, ExplicitModelPrimitiveTopology.LineStrip).WithSource(cSource);
        }

        public IResourceSource GetModelSource()
        {
            return new Source(FactoryResourceCache, this);
        }
    }
}