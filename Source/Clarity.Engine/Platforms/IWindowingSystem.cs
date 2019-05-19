using Clarity.Engine.Gui;

namespace Clarity.Engine.Platforms
{
    public interface IWindowingSystem
    {
        IRenderGuiControl RenderControl { get; }
    }
}