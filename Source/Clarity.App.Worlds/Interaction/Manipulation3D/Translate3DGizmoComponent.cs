using Clarity.Common.GraphicalGeometry;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.App.Worlds.Interaction.Manipulation3D
{
    public abstract class Translate3DGizmoComponent : SceneNodeComponentBase<Translate3DGizmoComponent>
    {
        public override void AmOnAttached()
        {
            Node.ChildNodes.Add(CreateDragAxisGizmo(Axis3D.X));
            Node.ChildNodes.Add(CreateDragAxisGizmo(Axis3D.Y));
            Node.ChildNodes.Add(CreateDragAxisGizmo(Axis3D.Z));
        }

        private static ISceneNode CreateDragAxisGizmo(Axis3D axis)
        {
            var node = AmFactory.Create<SceneNode>();
            var component = AmFactory.Create<DragAlongAxisGizmoComponent>();
            component.Axis = axis;
            node.Components.Add(component);
            return node;
        }
    }
}