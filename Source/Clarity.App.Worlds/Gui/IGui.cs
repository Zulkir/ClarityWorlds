using Clarity.Engine.Platforms;

namespace Clarity.App.Worlds.Gui
{
    public interface IGui : IWindowingSystem
    {
        void SwitchToPresentationMode();
        void SwitchToEditMode();
        void Run();
    }
}