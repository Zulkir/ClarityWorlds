// todo: reimplement
/*
using System.Linq;
using Clarity.Core.App.Aspects;
using Clarity.Core.App.WorldTree;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Aspects
{
    public class HyperLinkAspectCreationDialog : Dialog<HyperLinkEditAspect>
    {
        private readonly DropDown targetsControl;
        private readonly Button doneButton;

        public HyperLinkAspectCreationDialog(IWorldTreeService worldTreeService, HyperLinkEditAspect initial = null)
        {
            Title = "Choose Target";

            var possibleTargets = worldTreeService.World.EnumerateAllNodesDeep().Where(x => x.SearchAspect<IFocusNodeAspect>() != null).ToArray();
            targetsControl = new DropDown
            {
                Width = 80,
                DataStore = possibleTargets
            };

            doneButton = new Button { Text = "Done" };
            doneButton.Click += (s, a) =>
            {
                if (targetsControl.SelectedValue != null)
                {
                    var targetNodeName = ((ISceneNode)targetsControl.SelectedValue).Name;
                    var result = new HyperLinkEditAspect { TargetNodeName = targetNodeName };
                    Close(result);
                }
            };

            if (initial != null)
            {
                targetsControl.SelectedValue = possibleTargets.FirstOrDefault(x => x.Name == initial.TargetNodeName);
            }
            else
            {
                doneButton.Enabled = false;
                targetsControl.SelectedValueChanged += (s, a) =>
                {
                    doneButton.Enabled = true;
                };
            }
            
            var layout = new TableLayout(
                new TableRow(targetsControl),
                new TableRow(doneButton))
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            Content = layout;
        }
    }
}*/