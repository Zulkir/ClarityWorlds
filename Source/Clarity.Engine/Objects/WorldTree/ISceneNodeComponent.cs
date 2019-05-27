using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Objects.WorldTree
{
    public interface ISceneNodeComponent : IAmObject<ISceneNodeBound>, ISceneNodeBound
    {
        void Update(FrameTime frameTime);
        void OnRoutedEvent(IRoutedEvent evnt);
        void OnNodeEvent(IAmEventMessage message);
        // todo: user-friendly name
        // todo: GUI elements
    }
}