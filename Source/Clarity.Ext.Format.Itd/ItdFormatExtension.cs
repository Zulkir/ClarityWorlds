using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.ResourceTree.Assets;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Format.Itd
{
    public class ItdFormatExtension : IExtension
    {
        public string Name => "CGGC.Format.Itd";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IAssetLoader>().To<ItdModelLoader>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}