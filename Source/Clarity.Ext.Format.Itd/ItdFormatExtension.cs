using Clarity.App.Worlds.Assets;
using Clarity.Common.Infra.DependencyInjection;
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