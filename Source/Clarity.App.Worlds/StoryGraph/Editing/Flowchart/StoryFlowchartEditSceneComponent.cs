using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Elements.Materials;
using Clarity.Engine.Visualization.Elements.RenderStates;

namespace Clarity.App.Worlds.StoryGraph.Editing.Flowchart
{
    public abstract class StoryFlowchartEditSceneComponent : SceneNodeComponentBase<StoryFlowchartEditSceneComponent>,
        IFocusNodeComponent
    {
        private const float OffsetX = 0.2f;
        private const float OffsetY = 0.2f;

        private readonly Dictionary<ISceneNode, ISceneNode> gizmosByNodes;
        private readonly Dictionary<UnorderedPair<ISceneNode>, ISceneNode> edgeGizmos;
        private readonly IStandardMaterial routeMaterial;
        private readonly IStandardRenderState routeRenderState;
        private readonly IDefaultViewpointMechanism defaultViewpointMechanism;

        private FlowchartTunnelDigger flowchartTunnelDigger;

        protected StoryFlowchartEditSceneComponent()
        {
            gizmosByNodes = new Dictionary<ISceneNode, ISceneNode>();
            edgeGizmos = new Dictionary<UnorderedPair<ISceneNode>, ISceneNode>();

            defaultViewpointMechanism = new OrthoDefaultViewpointMechanism(Node,
                new PlaneOrthoBoundControlledCamera.Props
                {
                    Distance = 3f,
                    ZFar = 1000f,
                    ZNear = 0.01f
                });
            routeMaterial = StandardMaterial.New()
                .SetIgnoreLighting(true)
                .FromGlobalCache();
            routeRenderState = StandardRenderState.New()
                .SetLineWidth(3)
                .FromGlobalCache();
        }

        public void OnWorldRootChanged(ISceneNode newRoot, IStoryGraph newSg)
        {
            gizmosByNodes.Clear();
            edgeGizmos.Clear();
            Node.ChildNodes.Clear();
            CreateGizmosForSubtree(newRoot, 0);
            OnGraphUpdated(newSg);
        }

        public void OnStoryNodeAdded(ISceneNode parent, ISceneNode newNode, IStoryGraph newSg)
        {
            CreateGizmosForSubtree(newNode, newSg.Depths[newNode.Id]);
            OnGraphUpdated(newSg);
        }

        public void OnStoryNodeRemoved(ISceneNode parent, ISceneNode removedNode, IStoryGraph newSg)
        {
            DeleteGizmoSubtree(removedNode);
            OnGraphUpdated(newSg);
        }

        public void OnEdgeAdded(Pair<int> edge, IStoryGraph newSg)
        {
            OnGraphUpdated(newSg);
        }

        public void OnEdgeRemoved(Pair<int> edge, IStoryGraph newSg)
        {
            OnGraphUpdated(newSg);
        }

        private void OnGraphUpdated(IStoryGraph newSg)
        {
            UpdateRouteEdges(newSg);
            Rearrange(newSg);

            // digger only
            flowchartTunnelDigger = new FlowchartTunnelDigger(newSg);
            ApplyDigger();
            AdjustEdgeGizmos();
            FixNames();
        }

        private void CreateGizmosForSubtree(ISceneNode subtreeRoot, int depth)
        {
            if (subtreeRoot.ParentNode != null && !gizmosByNodes.ContainsKey(subtreeRoot.ParentNode))
                return;
            var storyAspect = subtreeRoot.SearchComponent<IStoryComponent>();
            if (storyAspect == null)
                return;
            var newGizmo = AmFactory.Factory.CreateWorldNodeWithComponent<StoryFlowchartNodeGizmoComponent>(out var newGizmoComponent);
            newGizmo.Name = $"NodeGizmo({subtreeRoot.Name})";
            newGizmoComponent.ReferencedNode = subtreeRoot;
            newGizmoComponent.Depth = depth;
            Node.ChildNodes.Add(newGizmo);
            gizmosByNodes.Add(subtreeRoot, newGizmo);
            //if (subtreeRoot.ParentNode != null)
            //    AddNewEdge(new UnorderedPair<ISceneNode>(subtreeRoot.ParentNode, subtreeRoot), parentChildMaterial);
            foreach (var child in subtreeRoot.ChildNodes)
                CreateGizmosForSubtree(child, depth + 1);
        }

        private void DeleteGizmoSubtree(ISceneNode subtreeRoot)
        {
            // todo: ChildNodes -> ImmediateStoryChildren
            foreach (var child in subtreeRoot.ChildNodes)
                DeleteGizmoSubtree(child);
            var gizmo = gizmosByNodes.TryGetRef(subtreeRoot);
            if (gizmo == null)
                return;
            gizmosByNodes.Remove(subtreeRoot);
            Node.ChildNodes.Remove(gizmo);
            var deathNote = edgeGizmos.Where(x => x.Key.Contains(subtreeRoot)).Select(x => x.Key).ToList();
            foreach (var key in deathNote)
            {
                Node.ChildNodes.Remove(edgeGizmos[key]);
                edgeGizmos.Remove(key);
            }
        }

        private void UpdateRouteEdges(IStoryGraph storyGraph)
        {
            var allRouteEdges = new HashSet<UnorderedPair<ISceneNode>>(storyGraph.Edges
                .Select(x => new UnorderedPair<ISceneNode>(storyGraph.NodeObjects[x.First], storyGraph.NodeObjects[x.Second])));
            var deathNote = edgeGizmos.Keys.Where(x => x.Second.ParentNode != x.First && !allRouteEdges.Contains(x)).ToList();
            foreach (var key in deathNote)
                DeleteEdge(key);
            foreach (var key in allRouteEdges.Where(x => !edgeGizmos.ContainsKey(x)))
                AddNewEdge(key, routeMaterial, routeRenderState);
        }

        private void AddNewEdge(UnorderedPair<ISceneNode> key, IStandardMaterial material, IStandardRenderState renderState)
        {
            var gizmo = AmFactory.Factory.CreateWorldNodeWithComponent<StoryFlowchartEdgeGizmoComponent>(out var component);
            gizmo.Name = $"EdgeGizmo({key.First}->{key.Second})";
            component.Material = material;
            component.RenderState = renderState;
            edgeGizmos.Add(key, gizmo);
            Node.ChildNodes.Add(gizmo);
        }

        private void DeleteEdge(UnorderedPair<ISceneNode> key)
        {
            var gizmo = edgeGizmos[key];
            edgeGizmos.Remove(key);
            Node.ChildNodes.Remove(gizmo);
        }
        
        private void Rearrange(IStoryGraph storyGraph)
        {
            var leafGizmos = storyGraph.Leaves.Select(x => gizmosByNodes[storyGraph.NodeObjects[x]]).ToList();
            {
                var width = 0f;
                foreach (var leafGizmo in leafGizmos)
                {
                    //leafGizmo.Transform.Own = Transform.Translation(width + OffsetX / 2, 0, 0);
                    var cGizmo = leafGizmo.GetComponent<StoryFlowchartNodeGizmoComponent>();
                    cGizmo.Depth = storyGraph.Depths[cGizmo.ReferencedNode.Id];
                    width += OffsetX;
                }
            }
            if (storyGraph.Depth > 1)
                ArrangeIntermediateNode(storyGraph.Root, storyGraph.Depth - 1, storyGraph);
        }

        private void AdjustEdgeGizmos()
        {
            foreach (var kvp in edgeGizmos)
            {
                var node1 = kvp.Key.First;
                var node2 = kvp.Key.Second;
                var edgeGizmo = kvp.Value;
                var component = edgeGizmo.GetComponent<StoryFlowchartEdgeGizmoComponent>();
                var pos1 = gizmosByNodes[node1].Transform.Offset;
                var pos2 = gizmosByNodes[node2].Transform.Offset;
                component.FirstPoint = pos1;
                component.MiddlePoint = (pos1 + pos2) / 2;
                if (pos1.Y == pos2.Y && Math.Abs(pos2.X - pos1.X) > 4)
                    component.MiddlePoint = component.MiddlePoint + Vector3.UnitY * 2;
                //component.MiddlePoint = pos1.Y > pos2.Y
                //    ? new Vector3(pos2.X, pos1.Y - OffsetY, 0)
                //    : new Vector3((pos1.X + pos2.X) / 2, pos1.Y - 0.33f * OffsetY * ((pos2.X - pos1.X) / OffsetX - 1), 0);
                component.LastPoint = pos2;
            }
        }

        private void ArrangeIntermediateNode(int node, int depthHeight, IStoryGraph storyGraph)
        {
            var aNode = storyGraph.Aspects[node];
            var gizmo = gizmosByNodes[aNode.Node];
            var firstLeafX = gizmosByNodes[storyGraph.Leaves.Select(x => storyGraph.NodeObjects[x]).First(x => x.IsDescendantOf(aNode.Node))].Transform.Offset.X;
            var lastLeafX = gizmosByNodes[storyGraph.Leaves.Select(x => storyGraph.NodeObjects[x]).Last(x => x.Node.IsDescendantOf(aNode.Node)).Node].Transform.Offset.X;
            gizmo.Transform = Transform.Translation((firstLeafX + lastLeafX) / 2, OffsetY * depthHeight, 0);
            if (depthHeight <= 1)
                return;
            foreach (var child in storyGraph.Children[node].Except(storyGraph.Leaves))
                ArrangeIntermediateNode(child, depthHeight - 1, storyGraph);
        }

        private void OffsetSubtree(ISceneNode subtreeRoot, ISceneNode gizmo, Vector3 offset)
        {
            gizmo.Transform *= Transform.Translation(offset);
            // todo: ChildNodes -> ImmediateStoryChildren
            foreach (var child in subtreeRoot.ChildNodes)
            {
                var childGizmo = gizmosByNodes.TryGetRef(child);
                if (childGizmo == null)
                    continue;
                OffsetSubtree(child, childGizmo, offset);
            }
        }

        private void ApplyDigger()
        {
            var offsetStack = new Stack<Vector2>();
            offsetStack.Push(Vector2.Zero);
            ApplyDiggerForNode(flowchartTunnelDigger.Graph.Root, offsetStack, flowchartTunnelDigger.Dig());
        }

        private void ApplyDiggerForNode(int node, Stack<Vector2> offsetStack, AaRectangle2[] rectangles)
        {
            var sg = flowchartTunnelDigger.Graph;
            var aNode = sg.Aspects[node];
            var rect = rectangles[node];
            offsetStack.Push(offsetStack.Peek() + rect.Center);
            var gizmo = gizmosByNodes[aNode.Node];
            var cGizmo = gizmo.GetComponent<StoryFlowchartNodeGizmoComponent>();
            gizmo.Transform = Transform.Translation(new Vector3(offsetStack.Peek(), 0));
            cGizmo.GlobalRectangle = new AaRectangle2(Vector2.Zero, rect.HalfWidth, rect.HalfHeight);
            if (sg.Children[node].Any())
            {
                cGizmo.UseThumbnail = false;
                foreach (var child in sg.Children[node])
                    ApplyDiggerForNode(child, offsetStack, rectangles);
            }
            else
            {
                cGizmo.UseThumbnail = true;
            }
            offsetStack.Pop();
        }

        private void FixNames()
        {
            foreach (var gizmo in gizmosByNodes.Values)
            {
                var component = gizmo.GetComponent<StoryFlowchartNodeGizmoComponent>();
                var name = $"Gizmo( {component.ReferencedNode.Name} )";
                if (gizmo.Name != name)
                    gizmo.Name = name;
            }
        }

        // Focus
        public IDefaultViewpointMechanism DefaultViewpointMechanism => defaultViewpointMechanism;
        public bool InstantTransition => false;
    }
}