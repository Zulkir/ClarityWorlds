using System;
using Clarity.Core.AppCore.UndoRedo;
using Clarity.Core.AppCore.WorldTree;
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
                DataStore = new[] { "AdditionalView", "Limiter0", "LimiterMax", "RotateOnDC" },
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
            /*
            if (component is InteractionEditAspect)
                return "Editing";
            if (component is HyperLinkEditAspect)
                return "HyperLink";
            if (component is PresentationWrapperAspect)
            {
                var caspect = (PresentationWrapperAspect)component;
                if (caspect.PresentationModeAspect is InteractionEditAspect)
                    return "Pres Editing";
            }
            if (component is InteractiveTransparentAspect)
                return "Transparency";
            if (component is ScrollAspect)
                return "Scrolling";*/
            return component.AmInterface.Name;
        }

        private void OnAddNewButtonClick(object sender, EventArgs eventArgs)
        {
            var aspectAlias = (string)aspectTypesControl.SelectedValue;
            switch (aspectAlias)
            {
                /*
                case "HyperLink":
                {
                    // todo: undo-redo
                    var dialog = new HyperLinkAspectCreationDialog(worldTreeService);
                    var aspect = dialog.ShowModal();
                    if (aspect == null)
                        return;
                    boundNode.Aspects.Add(aspect);
                    Actualize(boundNode);
                    break;
                }
                case "Pres Editing":
                {
                    var wrapperAspect = new PresentationWrapperAspect { PresentationModeAspect = new InteractionEditAspect() };
                    undoRedo.Common.Add(boundNode.Aspects, wrapperAspect);
                    break;
                }
                case "Transparency":
                {
                    var aspect = new InteractiveTransparentAspect();
                    undoRedo.Common.Add(boundNode.Aspects, aspect);
                    break;
                }
                case "Scrolling":
                {
                    var aspect = new ScrollAspect();
                    undoRedo.Common.Add(boundNode.Aspects, aspect);
                    break;
                }*/
                case "Limiter0":
                {
                    var component = AmFactory.Create<ModelLayerLimitComponent>();
                    undoRedo.Common.Add(boundNode.Components, component);
                    break;
                }
                case "LimiterMax":
                {
                    var component = AmFactory.Create<ModelLayerLimitComponent>();
                    component.InitialLimit = int.MaxValue;
                    undoRedo.Common.Add(boundNode.Components, component);
                    break;
                }
                case "RotateOnDC":
                {
                    var component = AmFactory.Create<RotateOnceComponent>();
                    undoRedo.Common.Add(boundNode.Components, component);
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