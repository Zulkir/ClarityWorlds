using System;
using Clarity.App.Worlds.Assets;
using Clarity.App.Worlds.External.Movies;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Infra.Files;
using Clarity.Engine.Gui.WindowQueries;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Resources;
using Eto.Drawing;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.Props
{
    public class PropsGuiGroupMovieRectangle : IPropsGuiGroup
    {
        public GroupBox GroupBox { get; }
        private readonly IUndoRedoService undoRedo;
        private readonly IAssetService assetService;
        private readonly IWindowQueryService windowQueryService;
        private readonly IMovieUrlLoader movieUrlLoader;
        private readonly Button cFile;
        private readonly Button cLink;
        private MovieRectangleComponent boundComponent;
        private OpenFileDialog movieOpenFileDialog;

        public PropsGuiGroupMovieRectangle(IUndoRedoService undoRedo, IAssetService assetService, IWindowQueryService windowQueryService, IMovieUrlLoader movieUrlLoader)
        {
            this.undoRedo = undoRedo;
            this.assetService = assetService;
            this.windowQueryService = windowQueryService;
            this.movieUrlLoader = movieUrlLoader;

            movieOpenFileDialog = new OpenFileDialog();
            movieOpenFileDialog.Filters.Add(new FileDialogFilter("Video Files", ".mkv", ".mp4", ".avi", ".mov", ".flv"));

            cFile = new Button { Text = "File" };
            cFile.Click += OnFileClicked;

            cLink = new Button { Text = "Link" };
            cLink.Click += OnLinkClicked;

            var layout = new TableLayout(
                new TableRow(cFile),
                new TableRow(cLink))
            {
                Padding = new Padding(5),
                Spacing = new Size(5, 5),
            };

            GroupBox = new GroupBox
            {
                Text = "Movie",
                Content = layout
            };
        }

        private void OnLinkClicked(object sender, EventArgs e)
        {
            if (boundComponent == null)
                return;
            if (!windowQueryService.TryQueryText("Stream URL", "", out var url))
                return;
            boundComponent.Movie = movieUrlLoader.Load(url);
        }

        private void OnFileClicked(object sender, EventArgs e)
        {
            if (boundComponent == null)
                return;

            var dialogResult = movieOpenFileDialog.ShowDialog(GroupBox);
            if (dialogResult != DialogResult.Ok)
                return;
            // todo: refactor to common code with DefaultMainForm tool commands
            var assetResult = assetService.Load(new AssetLoadInfo
            {
                FileSystem = ActualFileSystem.Singleton,
                LoadPath = movieOpenFileDialog.FileName,
                ReferencePath = movieOpenFileDialog.FileName,
                StorageType = AssetStorageType.CopyLocal
            });
            if (!assetResult.Successful)
                return;
            var asset = assetResult.Asset;

            IResource resource;
            if (asset.Resource == null)
            {
                MessageBox.Show($"The asset contains no resource.");
                return;
            }
            if (asset.Resource is ResourcePack pack)
            {
                resource = pack.MainSubresource;
                if (resource == null)
                {
                    MessageBox.Show($"The asset is a pack with no main subresource.");
                    return;
                }
            }
            else
            {
                resource = asset.Resource;
            }

            if (!(resource is IMovie movie))
            {
                MessageBox.Show($"The resource is not a movie");
                return;
            }

            boundComponent.Movie = movie;
        }

        public bool Actualize(ISceneNode node)
        {
            boundComponent = node.SearchComponent<MovieRectangleComponent>();
            return boundComponent != null;
        }
    }
}