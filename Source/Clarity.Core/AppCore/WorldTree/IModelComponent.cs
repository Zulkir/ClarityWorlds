using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Models.Flexible;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.WorldTree
{
    public interface IModelComponent : ISceneNodeComponent
    {
        IFlexibleModel Model { get; set; }
        Color4 Color { get; set; }
        bool IgnoreLighting { get; set; }
        bool NoSpecular { get; set; }
        bool SingleColor { get; set; }
        bool Ortho { get; set; }
        bool DontCull { get; set; }
        IImage Texture { get; set; }
    }
}