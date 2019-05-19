using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Core.AppCore.StoryGraph.Editing.Flowchart;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Utilities;

namespace Clarity.Core.AppCore.StoryGraph
{
    public class StoryService : IStoryService
    {
        private readonly IWorldTreeService worldTreeService;
        private readonly Dictionary<Type, IStoryLayout> layoutsByType;
        private readonly IScene editingScene;
        private readonly StoryFlowchartEditSceneComponent editSceneComponent;
        // todo: to LayoutInstances
        private readonly Dictionary<ISceneNode, IStoryLayout> layouts;
        private readonly List<object> transactions;
        private StoryServiceRootComponent rootComponent;

        public IScene EditingScene => editingScene.Scene;
        public IStoryGraph GlobalGraph { get; private set; }
        public IStoryLayoutInstance RootLayoutInstance { get; private set; }

        public event Action GraphChanged;

        public StoryService(IWorldTreeService worldTreeService, IRenderLoopDispatcher renderLoopDispatcher, IReadOnlyList<IStoryLayout> allLayouts)
        {
            this.worldTreeService = worldTreeService;
            layoutsByType = allLayouts.ToDictionary(x => x.Type, x => x);
            layouts = new Dictionary<ISceneNode, IStoryLayout>();
            transactions = new List<object>();
            editingScene = Scene.Create(AmFactory.Create<SceneNode>());
            editingScene.BackgroundColor = Color4.Black;
            editSceneComponent = AmFactory.Create<StoryFlowchartEditSceneComponent>();
            editingScene.Root.Components.Add(editSceneComponent);
            worldTreeService.UpdatedMed += OnWorldUpdated;
            renderLoopDispatcher.Update += editingScene.Root.Update;
        }

        private IReadOnlyList<Pair<int>> GetEdges() => (IReadOnlyList<Pair<int>>)rootComponent.Edges;
        private bool InsideTransaction => transactions.Count > 0;

        private void OnWorldUpdated(IAmEventMessage message)
        {
            if (InsideTransaction)
                return;
            if (message.Obj<WorldHolder>().ValueChanged(x => x.World, out _) ||
                message.Obj<IWorld>().ItemAddedOrRemoved(x => x.Scenes, out _) ||
                message.Obj<IScene>().ValueChanged(x => x.Root, out _))
            {
                if (worldTreeService.MainRoot != null)
                    OnWorldRootChanged(worldTreeService.MainRoot);
            }
            else if (message.Obj<ISceneNode>().ItemAdded(x => x.ChildNodes, out var subtreeAddedMessage))
            {
                if (subtreeAddedMessage.Item.HasComponent<IStoryComponent>())
                    OnStoryNodeAdded(subtreeAddedMessage.Object, subtreeAddedMessage.Item);
            }
            else if (message.Obj<ISceneNode>().ItemRemoved(x => x.ChildNodes, out var subtreeRemovedMessage))
            {
                if (subtreeRemovedMessage.Item.HasComponent<IStoryComponent>())
                    OnStoryNodeRemoved(subtreeRemovedMessage.Object, subtreeRemovedMessage.Item);
            }
            // todo: to StoryAspect.IsLayoutTypeChangedEvent(message)
            else if (message.Obj<StoryComponent>().ValueChanged(x => x.StartLayoutType, out var layoutTypeChangedMessage))
            {
                OnLayoutTypeChanged(layoutTypeChangedMessage.Object.Node, layoutTypeChangedMessage.NewValue);
            }
        }

        public void AddEdge(int from, int to)
        {
            rootComponent.Edges.Add(new Pair<int>(from, to));
            if (InsideTransaction)
                return;
            GlobalGraph = new StoryGraph(rootComponent.Node, GetEdges(), false);
            RootLayoutInstance = layouts.Values.Single().ArrangeAndDecorate(GlobalGraph);
            editSceneComponent.OnEdgeAdded(new Pair<int>(from, to), GlobalGraph);
            GraphChanged?.Invoke();
        }

        public void RemoveEdge(int from, int to)
        {
            rootComponent.Edges.Remove(new Pair<int>(from, to));
            if (InsideTransaction)
                return;
            GlobalGraph = new StoryGraph(rootComponent.Node, GetEdges(), false);
            RootLayoutInstance = layouts.Values.Single().ArrangeAndDecorate(GlobalGraph);
            editSceneComponent.OnEdgeRemoved(new Pair<int>(from, to), GlobalGraph);
            GraphChanged?.Invoke();
        }

        public void OnBeginTransaction(object obj)
        {
            transactions.Add(obj);
        }

        public void OnEndTransaction(object obj)
        {
            transactions.Remove(obj);
            if (InsideTransaction)
                return;
            OnWorldRootChanged(worldTreeService.MainRoot);
        }

        private void OnWorldRootChanged(ISceneNode newRoot)
        {
            rootComponent = newRoot.SearchComponent<StoryServiceRootComponent>();
            if (rootComponent == null)
            {
                rootComponent = AmFactory.Create<StoryServiceRootComponent>();
                newRoot.Components.Add(rootComponent);
            }

            var rootNode = newRoot;
            rootComponent = rootNode.SearchComponent<StoryServiceRootComponent>();
            if (rootComponent == null)
            {
                rootComponent = AmFactory.Create<StoryServiceRootComponent>();
                rootNode.Components.Add(rootComponent);
            }
            
            GlobalGraph = new StoryGraph(rootNode, GetEdges(), false);

            layouts.Clear();
            var aStory = rootNode.SearchComponent<IStoryComponent>();
            if (aStory?.StartLayoutType == null)
                throw new InvalidOperationException("Root SceneNode is expected to have a StoryAspect and be a LayoutRoot.");
            var layout = layoutsByType[aStory.StartLayoutType];
            layouts.Add(rootNode, layout);
            // todo: sub-layouts
            editSceneComponent.OnWorldRootChanged(newRoot, GlobalGraph);
            var rootSg = new StoryGraph(newRoot, (IReadOnlyList<Pair<int>>)rootComponent.Edges, true);
            RootLayoutInstance = layout.ArrangeAndDecorate(rootSg);
            GraphChanged?.Invoke();
        }

        private void OnStoryNodeAdded(ISceneNode parent, ISceneNode newNode)
        {
            var layoutRoot = SearchLayoutRoot(newNode);
            if (layoutRoot == newNode)
            {
                throw new NotImplementedException();
                //var layoutType = newNode.GetAspect<IStoryAspect>().StartLayoutType;
                //var layout = layoutsByType[layoutType];
                //layouts[newNode] = layout;
                //var sg = new StoryGraph(newNode, );
                //layout.ArrangeAndDecorate();
            }

            GlobalGraph = new StoryGraph(rootComponent.Node, GetEdges(), false);
            editSceneComponent.OnStoryNodeAdded(parent, newNode, GlobalGraph);
            RootLayoutInstance = layouts[rootComponent.Node].ArrangeAndDecorate(GlobalGraph);
            GraphChanged?.Invoke();
        }

        private void OnStoryNodeRemoved(ISceneNode parent, ISceneNode removedNode)
        {
            var layoutRoot = SearchLayoutRoot(removedNode);
            if (layoutRoot == removedNode)
            {
                throw new NotImplementedException();
                //var layoutType = newNode.GetAspect<IStoryAspect>().StartLayoutType;
                //var layout = layoutsByType[layoutType];
                //layouts[newNode] = layout;
                //var sg = new StoryGraph(newNode, );
                //layout.ArrangeAndDecorate();
            }
            var removedIds = removedNode.GetComponent<IStoryComponent>().EnumerateStoryAspectsDeep(false).Select(x => x.Node.Id).ToArray();
            rootComponent.Edges.RemoveWhere(x => removedIds.Contains(x.First) || removedIds.Contains(x.Second));
            GlobalGraph = new StoryGraph(rootComponent.Node, GetEdges(), false);
            editSceneComponent.OnStoryNodeRemoved(parent, removedNode, GlobalGraph);
            RootLayoutInstance = layouts[rootComponent.Node].ArrangeAndDecorate(GlobalGraph);
            GraphChanged?.Invoke();
        }

        private void OnLayoutTypeChanged(ISceneNode node, Type newLayoutType)
        {
            if (node != rootComponent.Node)
                throw new NotImplementedException();
            var layout = layoutsByType[newLayoutType];
            layouts[node] = layout;
            RootLayoutInstance = layout.ArrangeAndDecorate(GlobalGraph);
            GraphChanged?.Invoke();
        }

        private static ISceneNode SearchLayoutRoot(ISceneNode node)
        {
            var aStory = node.SearchComponent<IStoryComponent>();
            if (aStory == null)
                return null;
            if (aStory.IsLayoutRoot)
                return node;
            if (node.ParentNode == null)
                return null;
            return SearchLayoutRoot(node.ParentNode);
        }
    }
}