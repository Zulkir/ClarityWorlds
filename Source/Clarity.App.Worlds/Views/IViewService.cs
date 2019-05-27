using System;
using Clarity.Engine.Gui;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.Views
{
    public interface IViewService
    {
        IRenderGuiControl RenderControl { get; }
        ISceneNode SelectedNode { get; set; }
        IFocusableView MainView { get; }

        event EventHandler<ViewEventArgs> Update;

        void ChangeRenderingArea(IRenderGuiControl newRenderControl, IFocusableView mainView);
    }
}