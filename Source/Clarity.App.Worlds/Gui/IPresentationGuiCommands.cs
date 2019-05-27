using Clarity.Engine.Gui.Menus;

namespace Clarity.App.Worlds.Gui
{
    public interface IPresentationGuiCommands
    {
        IGuiCommand Move { get; }
        IGuiCommand Move3D { get; }
        IGuiCommand Rotate { get; }
        IGuiCommand Scale { get; }
        IGuiCommand Cut { get; }
        IGuiCommand Copy { get; }
        IGuiCommand Paste { get; }
        IGuiCommand Duplicate { get; }
        IGuiCommand Delete { get; }
        IGuiCommand FocusView { get; }
        IGuiCommand AddNewAdaptiveLayout { get; }
        IGuiCommand MoveUp { get; }
        IGuiCommand MoveDown { get; }
        IGuiCommand SetBorderCurve { get; }
        IGuiCommand MakeScenePortal { get; }
    }
}