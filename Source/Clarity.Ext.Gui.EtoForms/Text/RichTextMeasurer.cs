using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Text.Rich;
using Eto.Drawing;

namespace Clarity.Ext.Gui.EtoForms.Text
{
    public class RichTextMeasurer : IRichTextMeasurer
    {
        private readonly Graphics graphics;
        private readonly IFontFamilyCache fontFamilyCache;
        private readonly Dictionary<Pair<char, IRtSpanStyle>, Size2> charSizes;

        public RichTextMeasurer(IFontFamilyCache fontFamilyCache)
        {
            this.fontFamilyCache = fontFamilyCache;
            charSizes = new Dictionary<Pair<char, IRtSpanStyle>, Size2>();
            graphics = new Graphics(new Bitmap(2048, 512, PixelFormat.Format32bppRgba));
        }

        public Size2 MeasureString(string str, IRtSpanStyle style)
        {
            var etoFontFamily = fontFamilyCache.GetFontFamily(style.FontFamily);
            Converters.ToEto(style.FontDecoration, out var etoStyle, out var etoDecoration);
            var etoFont = new Font(etoFontFamily, style.Size, etoStyle, etoDecoration);
            return Converters.ToClarity(graphics.MeasureString(etoFont, str));
        }

        public Size2 GetCharSize(char ch, IRtSpanStyle style)
        {
            return charSizes.GetOrAdd(Tuples.Pair(ch, style), x => MeasureString(x.First.ToString(), x.Second));
        }
    }
}