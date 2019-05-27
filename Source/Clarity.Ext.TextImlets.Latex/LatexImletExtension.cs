using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.TextImlets.Latex
{
    public class LatexImletExtension : IExtension
    {
        public string Name => "TextImlets.Latex";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IRtEmbeddingHandler>().To<LatexRtEmbeddingHandler>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}
