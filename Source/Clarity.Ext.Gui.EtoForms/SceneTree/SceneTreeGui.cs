using System;
using System.Collections.Generic;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Interaction.Manipulation3D;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;
using JetBrains.Annotations;

namespace Clarity.Ext.Gui.EtoForms.SceneTree
{
    public class SceneTreeGui : ISceneTreeGui
    {
        private readonly IWorldTreeService worldTreeService;
        private readonly IViewService viewService;

        private readonly IPresentationGuiCommands commands;

        private readonly TreeView treeView;
        private readonly TreeItem rootItem;
        private readonly Dictionary<ISceneNode, TreeItem> itemIndex;

        private readonly Icon eyeIcon;
        private readonly Icon sceneIcon;
        private readonly Icon viewIcon;
        private readonly Icon layoutIcon;
        private readonly Icon entityIcon;
        private readonly Icon whiteIcon;

        private TreeItem focusedLayoutItem;
        private bool disabled;

        public TreeView TreeView => treeView;
        public TreeItem SelectedItem => (TreeItem)treeView.SelectedItem;
        public SceneTreeGuiItemTag SelectedItemTag => (SceneTreeGuiItemTag)SelectedItem.Tag;
        public ISceneNode SelectedNode => SelectedItemTag.Node;

        public SceneTreeGui(IEventRoutingService eventRoutingService, IWorldTreeService worldTreeService, IViewService viewService, 
            IPresentationGuiCommands commands, ICommonGuiObjects commonGuiObjects)
        {
            itemIndex = new Dictionary<ISceneNode, TreeItem>();
            this.worldTreeService = worldTreeService;
            this.viewService = viewService;
            this.commands = commands;

            eyeIcon = Icon.FromResource("Clarity.Ext.Gui.EtoForms.Resources.eye_icon.ico");
            sceneIcon = Icon.FromResource("Clarity.Ext.Gui.EtoForms.Resources.scene_icon.ico");
            viewIcon = Icon.FromResource("Clarity.Ext.Gui.EtoForms.Resources.view_icon.ico");
            layoutIcon = Icon.FromResource("Clarity.Ext.Gui.EtoForms.Resources.layout_icon.ico");
            entityIcon = Icon.FromResource("Clarity.Ext.Gui.EtoForms.Resources.entity_icon.ico");
            whiteIcon = Icon.FromResource("Clarity.Ext.Gui.EtoForms.Resources.white_icon.ico");

            rootItem = new TreeItem { Text = "GuiRoot", Expanded = true };
            treeView = new TreeView
            {
                Width = 250,
                DataStore = rootItem,
                ContextMenu = commonGuiObjects.SelectionContextMenu
            };
            //RebuildFromRoot();
            eventRoutingService.RegisterServiceDependency(typeof(ISceneTreeGui), typeof(IWorldTreeService));
            eventRoutingService.Subscribe<IWorldTreeUpdatedEvent>(typeof(ISceneTreeGui), nameof(OnWorldUpdated), OnWorldUpdated);
            eventRoutingService.Subscribe<IAppModeChangedEvent>(typeof(ISceneTreeGui), nameof(OnAppModeChanged), OnAppModeChanged);
            treeView.SelectionChanged += OnSelectionChanged;
            treeView.NodeMouseClick += OnNodeMouseClick;
            treeView.MouseDoubleClick += OnNodeMouseDoubleClick;

            viewService.Update += OnViewServiceUpdate;
        }

        private void OnAppModeChanged(IAppModeChangedEvent appModeChangedEvent)
        {
            if (appModeChangedEvent.NewAppMode == AppMode.Presentation)
            {
                disabled = true;
            }
            else
            {
                disabled = false;
                itemIndex.Clear();
                RebuildFromRoot();
                treeView.RefreshItem(rootItem);
            }
        }

        private void OnWorldUpdated(IWorldTreeUpdatedEvent evnt)
        {
            if (disabled)
                return;
            var message = evnt.AmMessage;
            if (message.Obj<WorldHolder>().ValueChanged(x => x.World, out _) ||
                message.Obj<IWorld>().ItemAddedOrRemoved(x => x.Scenes, out _) ||
                message.Obj<IScene>().ValueChanged(x => x.Root, out _))
            {
                //var cArgs = (AmValueChangedEventMessage<WorldModel, IWorld>)message;
                RebuildFromRoot();
                treeView.RefreshItem(rootItem);
            }
            else if (message.Obj<ISceneNode>().ItemAdded(x => x.ChildNodes, out var childAddedMsg))
            {
                var item = TryCreateNodeItem(childAddedMsg.Item);
                if (item == null)
                    return;
                var parentItem = itemIndex[childAddedMsg.Item.ParentNode];
                parentItem.Children.Insert(childAddedMsg.Object.ChildNodes.IndexOf(childAddedMsg.Item), item);
                treeView.RefreshItem(parentItem);
            }
            else if (message.Obj<ISceneNode>().ItemRemoved(x => x.ChildNodes, out var childRemovedMsg))
            {
                if (!itemIndex.TryGetValue(childRemovedMsg.Item, out var item))
                    return;
                foreach (var node in childRemovedMsg.Item.EnumerateSceneNodesDeep())
                    itemIndex.Remove(node);
                var parentItem = (TreeItem)item.Parent;
                parentItem.Children.Remove(item);
                treeView.RefreshItem(parentItem);
            }
            else if (message.Obj<ISceneNode>().ValueChanged(x => x.Name, out var nameChangedMsg))
            {
                var item = itemIndex[nameChangedMsg.Object];
                item.Text = nameChangedMsg.Object.Name;
                treeView.RefreshItem(item);
            }
        }

        private void RebuildFromRoot()
        {
            rootItem.Children.Clear();
            if (worldTreeService.MainRoot != null)
                rootItem.Children.Add(TryCreateNodeItem(worldTreeService.MainRoot));
        }

        [CanBeNull]
        private TreeItem TryCreateNodeItem(ISceneNode node)
        {
            var item = new TreeItem
            {
                Text = node.Name,
                Tag = new SceneTreeGuiItemTag(node),
                Image = GetDefaultIconFor(node),
            };
            itemIndex[node] = item;
            foreach (var child in node.ChildNodes)
            {
                var childItem = TryCreateNodeItem(child);
                if (childItem != null)
                    item.Children.Add(childItem);
            }
            return item;
        }

        private void OnNodeMouseClick(object sender, TreeViewItemEventArgs treeViewItemEventArgs) => 
            treeView.SelectedItem = treeViewItemEventArgs.Item;

        private void OnNodeMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (SelectedNode.HasComponent<IFocusNodeComponent>())
                commands.FocusView.Execute();
        }

        private void OnSelectionChanged(object sender, EventArgs eventArgs)
        {
            viewService.SelectedNode = SelectedItemTag.Node;
        }

        private void OnViewServiceUpdate(object sender, ViewEventArgs viewEventArgs)
        {
            if (disabled)
                return;

            if (viewEventArgs.Type == ViewEventType.FocusedNodeChanged || viewEventArgs.Type == ViewEventType.ViewportChanged)
            {
                if (focusedLayoutItem != null)
                {
                    var tag = (SceneTreeGuiItemTag)focusedLayoutItem.Tag;
                    focusedLayoutItem.Image = GetDefaultIconFor(tag.Node);
                    foreach (var child in focusedLayoutItem.Children)
                        child.Expanded = false;
                    treeView.RefreshItem(focusedLayoutItem);
                }

                var focusedLayout = viewService.MainView.FocusNode;
                focusedLayoutItem = itemIndex[focusedLayout];
                focusedLayoutItem.Image = eyeIcon;
                treeView.RefreshItem(focusedLayoutItem);
            }
            else if (viewEventArgs.Type == ViewEventType.SelectedNodeChanged)
            {
                var selectedNode = viewService.SelectedNode;
                if (selectedNode != null)
                {
                    var item = itemIndex.TryGetRef(selectedNode);
                    treeView.SelectedItem = item;
                }
                else
                {
                    treeView.SelectedItem = null;
                }
            }
        }

        private Icon GetDefaultIconFor(ISceneNode node)
        {
            if (node.AmParent is IScene)
                return sceneIcon;
            if (node.HasComponent<StoryComponent>())
                return layoutIcon;
            if (node.HasComponent<ITransformable3DComponent>() || node.HasComponent<RectangleComponent>())
                return entityIcon;
            return whiteIcon;
        }
    }
}