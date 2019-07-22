using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Media.Skyboxes;
using Clarity.Engine.Platforms;

namespace Clarity.Engine.Objects.WorldTree 
{
    public interface IScene : IAmObject, ISceneNodeParent
    {
        IWorld World { get; }

        string Name { get; set; }
        Color4 BackgroundColor { get; set; }
        ISkybox Skybox { get; set; }
        ISceneNode Root { get; }
        IList<ISceneNode> AuxuliaryNodes { get; }

        void Update(FrameTime frameTime);
        void OnRoutedEvent(IRoutedEvent evnt);
    }
}