using Clarity.App.Worlds.Media.Media2D;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.App.Worlds.Interaction.RectangleManipulation
{
    public abstract class EditRectangleGizmoComponent : SceneNodeComponentBase<EditRectangleGizmoComponent>
    {
        public ISceneNode RectangleNode { get; set; }
        public AaRectangle2 Rectangle => RectangleNode.GetComponent<IRectangleComponent>().Rectangle;

        public static EditRectangleGizmoComponent Create(ISceneNode rectangleNode)
        {
            var result = AmFactory.Create<EditRectangleGizmoComponent>();
            result.RectangleNode = rectangleNode;
            return result;
        }

        public override void Update(FrameTime frameTime)
        {
            Node.Transform = RectangleNode.GlobalTransform;
        }
    }
}