namespace Clarity.Common.Shapes
{
    public interface IShapeCollisionChecker
    {
        bool IsSuitedForShapes(IShape3D shape1, IShape3D shape2);
        bool ShapesAreColliding(IShape3D shape1, IShape3D shape2);
    }
}