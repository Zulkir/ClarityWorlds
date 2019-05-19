using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Gui;

namespace Clarity.Core.AppCore.SaveLoad
{
    public interface IDefaultStateInitializer
    {
        void InitializeAll();
        void ResetEditingViewports(IRenderGuiControl renderControl, IFocusNodeComponent aMainFocusNode);
    }
}