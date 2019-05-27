using Clarity.App.Worlds.Views;
using Clarity.Engine.Gui;

namespace Clarity.App.Worlds.SaveLoad
{
    public interface IDefaultStateInitializer
    {
        void InitializeAll();
        void ResetEditingViewports(IRenderGuiControl renderControl, IFocusNodeComponent aMainFocusNode);
    }
}