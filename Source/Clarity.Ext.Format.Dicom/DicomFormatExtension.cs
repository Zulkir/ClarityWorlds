using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Format.Dicom
{
    public class DicomFormatExtension : IExtension
    {
        public string Name => "Technion.Format.Dicom";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IAssetLoader>().To<DicomModelLoader>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}
