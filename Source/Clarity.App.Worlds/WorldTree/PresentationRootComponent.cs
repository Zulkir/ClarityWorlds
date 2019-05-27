using System;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;

namespace Clarity.App.Worlds.WorldTree
{
    public abstract class PresentationRootComponent : SceneNodeComponentBase<PresentationRootComponent>, IPresentationRootComponent
    {
        public abstract Guid PresentationGuid { get; set; }
        public abstract Scene HudScene { get; set; }

        public static PresentationRootComponent Create(Guid presentationGuid)
        {
            var self = AmFactory.Create<PresentationRootComponent>();
            self.PresentationGuid = presentationGuid;
            return self;
        }
    }
}