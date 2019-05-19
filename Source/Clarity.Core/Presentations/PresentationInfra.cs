using System;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.Presentations
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
        public PlacementPlaneComponent PlacementNodeAspect => node.SearchComponent<PlacementPlaneComponent>() ?? ParentInfra?.PlacementNodeAspect;
    }
}