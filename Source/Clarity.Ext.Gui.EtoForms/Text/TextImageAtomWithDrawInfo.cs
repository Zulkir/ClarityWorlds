using Eto.Drawing;

namespace Clarity.Ext.Gui.EtoForms.Text
{
    public struct TextImageAtomWithDrawInfo
    {
        public TextImageAtom Atom;
        public Font Font;
        public Color Color;
        public float Offset;
        public SizeF Size;

        public TextImageAtomWithDrawInfo(TextImageAtom atom, Font font, Color color, float offset, SizeF size)
        {
            Atom = atom;
            Font = font;
            Color = color;
            Offset = offset;
            Size = size;
        }
    }
}