using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Visualization.Views;
using JetBrains.Annotations;

namespace Clarity.Engine.Visualization.Viewports
{
    // todo: make no longer AmObject
    public interface IViewport : IAmObject
    {
        int Left { get; }
        int Top { get; }
        int Width { get; }
        int Height { get; }
        float AspectRatio { get; }
        
        [NotNull]
        IView View { get; set; }

        void OnResized(float left, float top, float width, float height);
    }
}