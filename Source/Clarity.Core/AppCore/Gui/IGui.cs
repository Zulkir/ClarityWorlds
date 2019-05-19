using Clarity.Engine.Platforms;

namespace Clarity.Core.AppCore.Gui
{
    public interface IGui : IWindowingSystem
    {
        void SwitchToPresentationMode();
        void SwitchToEditMode();
        void Run();
    }
}