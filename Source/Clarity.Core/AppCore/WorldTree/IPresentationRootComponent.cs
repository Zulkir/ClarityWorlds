using System;
using Clarity.Engine.Objects.WorldTree;
using JetBrains.Annotations;

namespace Clarity.Core.AppCore.WorldTree 
{
    public interface IPresentationRootComponent : ISceneNodeComponent
    {
        Guid PresentationGuid { get; }
        [CanBeNull] Scene HudScene { get; }
    }
}