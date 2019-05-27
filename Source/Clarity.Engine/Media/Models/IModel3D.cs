using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Models
{
    public interface IModel3D : IResource
    {
        int PartCount { get; }
        Sphere BoundingSphere { get; }

        // todo:
        //IExplicitModel ToExplicitModel();
    }
}