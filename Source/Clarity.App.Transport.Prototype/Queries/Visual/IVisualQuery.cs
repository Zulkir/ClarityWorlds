using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

namespace Clarity.App.Transport.Prototype.Queries.Visual
{
    public interface IVisualQuery : IQuery
    {
        ISceneNode SceneNode { get; }
        void Update(FrameTime frameTime);
    }
}