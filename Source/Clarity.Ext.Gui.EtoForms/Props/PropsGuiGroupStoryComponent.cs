using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryGraph.Editing.Flowchart;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.Objects.WorldTree;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupStoryComponent : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly IWorldTreeService worldTreeService;
        private readonly IStoryService storyService;
        private readonly GroupBox cExplicitConnections;
        private readonly DropDown cDefaultAdaptiveStyle;
        private readonly CheckBox cShowAux1;
        private readonly CheckBox cShowAux2;
        private readonly Dictionary<string, Type> typeDict;
        private StoryComponent boundComponent;

        public PropsGuiGroupStoryComponent(IUndoRedoService undoRedo, IReadOnlyList<IStoryLayout> storyLayouts, IWorldTreeService worldTreeService, IStoryService storyService)
        {
            this.undoRedo = undoRedo;
            this.storyService = storyService;
            this.worldTreeService = worldTreeService;

            cDefaultAdaptiveStyle = new DropDown();
            typeDict = storyLayouts.ToDictionary(x => x.UserFriendlyName, x => x.Type);
            typeDict.Add("none", null);
            cDefaultAdaptiveStyle.DataStore = typeDict.Keys;
            cDefaultAdaptiveStyle.SelectedValueChanged += OnDefaultAdaptiveStyleChanged;

            cShowAux1 = new CheckBox{Text = "Aux1"};
            cShowAux2 = new CheckBox{Text = "Aux2"};

            cShowAux1.CheckedChanged += OnShowAux1Changed;
            cShowAux2.CheckedChanged += OnShowAux2Changed;

            cExplicitConnections = new GroupBox
            {
                Text = "Explicit Connections"
            };
            
            var cRedecorate = new Button { Text = "Redecorate" };
            cRedecorate.Click += (s, a) =>
            {
                storyService.OnBeginTransaction(this);
                storyService.OnEndTransaction(this);
            };

            var layout = new TableLayout(
                new TableRow(new Label { Text = "Default Adaptive Style" }, cDefaultAdaptiveStyle),
                new TableRow(cShowAux1, cShowAux2),
                new TableRow(cExplicitConnections),
                new TableRow(cRedecorate))
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
            
            GroupBox = new GroupBox
            {
                Text = "Story",
                Content = layout
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = null;

            // todo: remove since we no longer select gizmos
            var gizmoComponent = node.SearchComponent<StoryFlowchartNodeGizmoComponent>();
            var storyNode = gizmoComponent != null ? gizmoComponent.ReferencedNode : node;
            var storyComponent = storyNode.SearchComponent<StoryComponent>();
            if (storyComponent == null)
                return false;

            cShowAux1.Checked = storyComponent.ShowAux1;
            cShowAux2.Checked = storyComponent.ShowAux2;

            cDefaultAdaptiveStyle.SelectedKey = typeDict.First(x => x.Value == storyComponent.StartLayoutType).Key;
            cExplicitConnections.Content = BuildConnectionsLayout(node);
            boundComponent = storyComponent;
            return true;
        }

        private TableLayout BuildConnectionsLayout(ISceneNode selectedNode)
        {
            var listLayout = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            if (storyService.GlobalGraph.Children[selectedNode.Id].Any())
                return listLayout;

            foreach (var nextId in storyService.GlobalGraph.Next[selectedNode.Id])
            {
                var nextIdLoc = nextId;
                var next = worldTreeService.GetById(nextId);
                var label = new Label { Text = next.Name };
                var removeButton = new Button { Text = "X", Width = 20, Height = 20 };
                removeButton.Click += (s, a) =>
                {
                    // todo: undo/redo
                    storyService.RemoveEdge(selectedNode.Id, nextIdLoc);
                    Actualize(selectedNode);
                };
                listLayout.Rows.Add(new TableRow(new TableCell(label, true), new TableCell(removeButton)));
            }

            return listLayout;
        }

        private void OnDefaultAdaptiveStyleChanged(object sender, EventArgs eventArgs)
        {
            if (boundComponent == null)
                return;
            var type = typeDict[(string)cDefaultAdaptiveStyle.SelectedValue];
            boundComponent.StartLayoutType = type;
            undoRedo.OnChange();
        }

        private void OnShowAux1Changed(object sender, EventArgs eventArgs)
        {
            if (boundComponent != null)
                boundComponent.ShowAux1 = cShowAux1.Checked ?? false;
        }

        private void OnShowAux2Changed(object sender, EventArgs eventArgs)
        {
            if (boundComponent != null)
                boundComponent.ShowAux2 = cShowAux2.Checked ?? false;
        }
    }
}