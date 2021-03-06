using System.Collections.Generic;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.External.Movies;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.Gui.WindowQueries;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGui : IPropsGui
    {
        public Panel PropsPanel { get; }
        private readonly IViewService viewService;

        private readonly IPropsGuiGroup[] groups;

        public PropsGui(IViewService viewService, IUndoRedoService undoRedo, IAssetService assetService, IWorldTreeService worldTreeService, 
            IReadOnlyList<IStoryLayout> storyLayouts, IStoryService storyService, IEmbeddedResources embeddedResources, IWindowQueryService windowQueryService,
            IMovieUrlLoader movieUrlLoader)
        {
            this.viewService = viewService;
            
            PropsPanel = new Panel
            {
                Content = new TableLayout(),
                Width = 250,
                Height = 300
            };

            groups = new IPropsGuiGroup[]
            {
                new PropsGuiGroupCommon(undoRedo),
                new PropsGuiGroupFluidSimulation(undoRedo), 
                new PropsGuiGroupStoryComponent(undoRedo, storyLayouts, worldTreeService, storyService), 
                new PropsGuiGroupMovieRectangle(undoRedo, assetService, windowQueryService, movieUrlLoader),
                new PropsGuiGroupHighlightOnMouse(undoRedo),
                new PropsGuiGroupComponents(undoRedo),
            };

            viewService.Update += OnViewServiceUpdate;
        }

        private void OnViewServiceUpdate(object sender, ViewEventArgs viewEventArgs)
        {
            if (viewEventArgs.Type == ViewEventType.SelectedNodeChanged)
            {
                var layout = BuildLayoutFor(viewService.SelectedNode);
                PropsPanel.Content = layout;
            }
        }

        private TableLayout BuildLayoutFor(ISceneNode node)
        {
            var layout = new TableLayout
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5)
            };

            if (node != null)
            {
                foreach (var grp in groups)
                    if (grp.Actualize(node))
                        layout.Rows.Add(new TableRow(new TableCell(grp.GroupBox)));
            }

            layout.Rows.Add(new TableRow());
            
            return layout;
        }
    }
}