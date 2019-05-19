using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.StoryLayout.Building
{
    public class BuildingStoryLayoutExtension : IExtension
    {
        public string Name => "Clarity.Ext.StoryLayout.Building";

        public void Bind(IDiContainer di)
        {
            di.BindMulti<IStoryLayout>().To<BuildingStoryLayout>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}
