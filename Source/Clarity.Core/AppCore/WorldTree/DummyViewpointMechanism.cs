using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;

namespace Clarity.Core.AppCore.WorldTree 
{
    public class DummyViewpointMechanism : IDefaultViewpointMechanism 
    {
        public ICamera FixedCamera { get; }

        public DummyViewpointMechanism(ISceneNode node)
        {
            FixedCamera = new FixedCamera(node, Vector3.Zero, Vector3.UnitX, Vector3.UnitY, 1, 1, 2);
        }

        public ICamera CreateControlledViewpoint() => FixedCamera;
    }
}