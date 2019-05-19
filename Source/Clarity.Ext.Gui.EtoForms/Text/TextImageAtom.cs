using Clarity.Engine.Media.Text.Rich;

namespace Clarity.Ext.Gui.EtoForms.Text
{
    public struct TextImageAtom
    {
        public string Text;
        public IRtSpanStyle Style;
        public bool IsBullet;

        public TextImageAtom(string text, IRtSpanStyle style, bool isBullet = false)
        {
            Text = text;
            Style = style;
            IsBullet = isBullet;
        }
    }
}