using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Objects.WorldTree
{
    public interface IWorld : IAmObject
    {
        IList<IScene> Scenes { get; }
        IList<string> Tags { get; }
        IPropertyBag Properties { get; }
        int NextId { get; }

        ISceneNode GetNodeById(int id);
        bool TryGetNodeById(int id, out ISceneNode node);

        void Update(FrameTime frameTime);
        void OnRoutedEvent(IRoutedEvent evnt);
    }
}