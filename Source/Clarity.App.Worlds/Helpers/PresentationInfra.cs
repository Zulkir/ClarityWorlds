using System;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Views;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Helpers
{
    public struct PresentationInfra
    {
        private readonly ISceneNode node;
        
        public PresentationInfra(ISceneNode node)
        {
            this.node = node ?? throw new ArgumentNullException(nameof(node));
        }

        private PresentationInfra? ParentInfra => node.ParentNode != null ? new PresentationInfra(node.ParentNode) : (PresentationInfra?)null;
        public ISceneNode ClosestFocusNode => node.HasComponent<IFocusNodeComponent>() ? node : ParentInfra?.ClosestFocusNode;
        public IPlacementComponent Placement => node.SearchComponent<IPlacementComponent>() ?? ParentInfra?.Placement;
    }
}