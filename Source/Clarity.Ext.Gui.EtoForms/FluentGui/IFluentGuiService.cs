using Clarity.Engine.Platforms;
using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentGuiService
    {
        Control RootEtoControl { get; }
        void Update(FrameTime frameTime);
    }
}