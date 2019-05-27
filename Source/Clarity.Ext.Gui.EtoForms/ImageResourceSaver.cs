using Clarity.Common.Infra.Files;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Resources;
using Clarity.Engine.Resources.SaveLoad;
using Eto.Drawing;

namespace Clarity.Ext.Gui.EtoForms
{
    public class ImageResourceSaver : IResourceSaver
    {
        public bool CanSaveNatively(IResource resource) => resource is IImage;
        public bool CanSaveByConversion(IResource resource) => resource is IImage;
        public string SuggestFileName(IResource resource, string resourceName = null) => (resourceName ?? "image") + ".png";

        public void Save(IResource resource, IFileSystem fileSystem, string path)
        {
            var image = (IImage)resource;
            var bitmap = FromEtoImage.EtoBitmapFromRaw(image.Size, image.GetRawData());
            using (var stream = fileSystem.OpenWriteNew(path))
                bitmap.Save(stream, ImageFormat.Png);
        }
    }
}