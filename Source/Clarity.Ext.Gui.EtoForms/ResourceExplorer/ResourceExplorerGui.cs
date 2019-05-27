using System.Linq;
using Clarity.App.Worlds.Assets;
using Clarity.Engine.Resources;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.ResourceExplorer
{
    public class ResourceExplorerGui : IResourceExplorerGui
    {
        public TreeView TreeView { get; }
        private TreeItem rootItem;
        private TreeItem assetsItem;

        public ResourceExplorerGui(IAssetService assetService)
        {
            rootItem = new TreeItem
            {
                Text = "Root",
                Tag = null,
                Expanded = true
            };
            TreeView = new TreeView
            {
                Width = 250,
                DataStore = rootItem
            };
            assetsItem = new TreeItem
            {
                Text = "Assets",
                Expanded = true
            };
            foreach (var asset in assetService.Assets.Values)
                assetsItem.Children.Add(BuildAssetTreeItem(asset));
            TreeView.RefreshItem(rootItem);
        }

        private TreeItem BuildAssetTreeItem(IAsset asset)
        {
            var item = new TreeItem
            {
                Text = asset.Name,
                Tag = asset
            };
            item.Children.Add(BuildAssetResourceTreeItem(asset.Resource, "Resource"));
            return item;
        }

        private TreeItem BuildAssetResourceTreeItem(IResource resource, string name)
        {
            var item = new TreeItem
            {
                Text = name,
                Tag = resource
            };
            foreach (var kvp in resource.Subresources.OrderBy(x => x.Key))
                item.Children.Add(BuildAssetResourceTreeItem(kvp.Value, kvp.Key));
            return item;
        }
    }
}