using Clarity.App.Worlds.SaveLoad.Import;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Import.Pptx
{
    public class PptxImportExtension : IExtension
    {
        public string Name => "PowerPoint Import";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IPresentationImporter>().To<PptxPresentationImporter>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}