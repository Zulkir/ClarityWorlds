using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Cameras;

namespace Clarity.Engine.Visualization.Views 
{
    public class ViewLayer : IViewLayer 
    {
        public IScene VisibleScene { get; set; }
        public ICamera Camera { get; set; }
    }
}