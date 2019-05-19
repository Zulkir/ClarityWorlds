using Eto.Drawing;

namespace Clarity.Ext.Gui.EtoForms.Text
{
    public interface IFontFamilyCache
    {
        FontFamily GetFontFamily(string fontFamilyName);
    }
}