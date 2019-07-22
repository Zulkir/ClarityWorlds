using System;
using Clarity.App.Worlds.External.WarpScrolling;
using Clarity.App.Worlds.Misc.HighlightOnMouse;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.WorldTree.MiscComponents;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupComponents : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly DropDown aspectTypesControl;
        private readonly Button addNewButton;
        private ISceneNode boundNode;

        public PropsGuiGroupComponents(IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;

            aspectTypesControl = new DropDown
            {
                Width = 120,
                DataStore = new[] { "RotateOnDC", "ManipOnPresent", "WarpScroll", "HighlightOnMouseOver" },
                SelectedIndex = 0
            };

            addNewButton = new Button {Text = "Add", Width = 50 };
            addNewButton.Click += OnAddNewButtonClick;

            var content = BuildLayout(null);

            GroupBox = new GroupBox
            {
                Text = "Components",
                Content = content
            };
        }

        public bool Actualize(ISceneNode node)
        {
            boundNode = null;

            var newNode = node;
            if (newNode == null)
                return false;
            
            GroupBox.Content = BuildLayout(newNode);

            boundNode = newNode;
            return true;
        }

        private TableLayout BuildLayout(ISceneNode node)
        {
            var listLayout = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            if (node != null)
            {
                foreach (var component in node.Components)
                {
                    var label = new Label { Text = GetAspectName(component) };
                    var editButton = new Button { Text = "E", Width = 20 };
                    editButton.Click += (s, e) =>
                    {
                        TryEditAspect(component);
                    };
                    var removeButton = new Button { Text = "R", Width = 20 };
                    removeButton.Click += (s, a) =>
                    {
                        node.Components.Remove(component);
                        Actualize(node);
                    };
                    listLayout.Rows.Add(new TableRow(label, editButton, removeButton));
                }
            }

            var addRow = new TableLayout(new TableRow(aspectTypesControl, addNewButton))
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            return new TableLayout(listLayout, addRow)
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };
        }

        private static string GetAspectName(ISceneNodeComponent component)
        {
            return component.AmInterface.Name;
        }

        private void OnAddNewButtonClick(object sender, EventArgs eventArgs)
        {
            var aspectAlias = (string)aspectTypesControl.SelectedValue;
            switch (aspectAlias)
            {
                case "RotateOnDC":
                {
                    var component = AmFactory.Create<RotateOnceComponent>();
                    boundNode.Components.Add(component);
                    undoRedo.OnChange();
                    break;
                }
                case "ManipOnPresent":
                {
                    var component = AmFactory.Create<ManipulateInPresentationComponent>();
                    boundNode.Components.Add(component);
                    undoRedo.OnChange();
                    break;
                }
                case "WarpScroll":
                {
                    var component = AmFactory.Create<WarpScrollComponent>();
                    boundNode.Components.Add(component);
                    undoRedo.OnChange();
                    break;
                }
                case "HighlightOnMouseOver":
                {
                    var component = AmFactory.Create<HighlightOnMouseComponent>();
                    boundNode.Components.Add(component);
                    undoRedo.OnChange();
                    break;
                }
            }
        }

        private bool TryEditAspect(ISceneNodeComponent component)
        {
            /*
            if (aspect is HyperLinkEditAspect)
            {
                // todo: undo-redo
                var cAspect = (HyperLinkEditAspect)aspect;
                var result = new HyperLinkAspectCreationDialog(worldTreeService, cAspect).ShowModal();
                if (result != null)
                {
                    cAspect.TargetNodeName = result.TargetNodeName;
                    return true;
                }
            }
            else if (aspect is ISceneAspect)
            {
                
            }*/
            return false;
        }
    }
}