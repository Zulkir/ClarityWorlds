using Eto.Forms;

namespace Clarity.Ext.Gui.EtoForms.FluentGui
{
    public interface IFluentControl
    {
        Control EtoControl { get; }
        bool IsVisible { get; }
        void Update();
    }
}