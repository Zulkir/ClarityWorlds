using System;
using System.Collections.Generic;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using JetBrains.Annotations;

namespace Clarity.App.Worlds.StoryGraph
{
    public interface IStoryComponent : ISceneNodeComponent
    {
        [CanBeNull]
        Type StartLayoutType { get; set; }
        bool IsLayoutRoot { get; }

        Color4? BackgroundColor { get; set; }
        bool InstantTransition { get; set; }
        bool SkipOrder { get; set; }
        bool HideMain { get; set; }
        bool ShowAux1 { get; set; }
        bool ShowAux2 { get; set; }
        bool ShowAux3 { get; set; }
        bool ShowAux4 { get; set; }
        bool ShowAux5 { get; set; }

        IImage GetThumbnail();
        void InvalidateThumbnails();

        IEnumerable<IStoryComponent> EnumerateImmediateStoryChildren(bool sameLayoutOnly);
        IEnumerable<IStoryComponent> EnumerateStoryAspectsDeep(bool sameLayoutOnly);

        void SetDynamicParts(StoryNodeDynamicParts dynamicParts);
    }
}