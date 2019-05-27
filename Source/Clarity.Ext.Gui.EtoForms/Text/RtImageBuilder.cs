using System;
using System.Linq;
using Clarity.Engine.Media.Text.Rich;
using Eto.Drawing;
using EFontDecoration = Eto.Drawing.FontDecoration;
using CFontDecoration = Clarity.Engine.Media.Text.Rich.FontDecoration;

namespace Clarity.Ext.Gui.EtoForms.Text
{
    public class RtImageBuilder : IRtImageBuilder
    {
        private readonly IFontFamilyCache fontFamilyCache;

        public RtImageBuilder(IFontFamilyCache fontFamilyCache)
        {
            this.fontFamilyCache = fontFamilyCache;
        }

        public byte[] BuildImageRgba(IRichTextBox textBox)
        {
            var text = textBox.Text;
            var layout = textBox.Layout;

            var bitmap = new Bitmap(textBox.Size.Width, textBox.Size.Height, PixelFormat.Format32bppRgba);
            var graphics = new Graphics(bitmap);
            var backgroundColor = text.Style.BackgroundColor;
            graphics.Clear(new Color(backgroundColor.R, backgroundColor.G, backgroundColor.B, backgroundColor.A));

            foreach (var lspan in layout.LayoutSpans.Concat(layout.ExternalLayoutSpans))
            {
                var style = lspan.Style;
                var rectText = lspan.Text;
                var etoFontFamily = fontFamilyCache.GetFontFamily(style.FontFamily);
                var etoFont = new Font(etoFontFamily, style.Size, ConvertFontStyle(style.FontDecoration), ConvertFontDecoration(style.FontDecoration));
                var etoColor = new Color(style.TextColor.R, style.TextColor.G, style.TextColor.B, style.TextColor.A);
                graphics.DrawText(etoFont, etoColor, lspan.Bounds.MinX, lspan.Bounds.MinY, rectText);
            }
            graphics.Flush();

            return FromEtoImage.GetRawData(bitmap);
        }
        
        private static FontStyle ConvertFontStyle(CFontDecoration fontDecoration)
        {
            switch (fontDecoration & (CFontDecoration.Bold | CFontDecoration.Italic))
            {
                case CFontDecoration.None:
                    return FontStyle.None;
                case CFontDecoration.Bold:
                    return FontStyle.Bold;
                case CFontDecoration.Italic:
                    return FontStyle.Italic;
                case CFontDecoration.Bold | CFontDecoration.Italic:
                    return FontStyle.Bold | FontStyle.Italic;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fontDecoration));
            }
        }

        private static EFontDecoration ConvertFontDecoration(CFontDecoration fontDecoration)
        {
            switch (fontDecoration & (CFontDecoration.Underline | CFontDecoration.Strikethrough))
            {
                case CFontDecoration.None:
                    return EFontDecoration.None;
                case CFontDecoration.Underline:
                    return EFontDecoration.Underline;
                case CFontDecoration.Strikethrough:
                    return EFontDecoration.Strikethrough;
                case CFontDecoration.Underline | CFontDecoration.Strikethrough:
                    return EFontDecoration.Underline | EFontDecoration.Strikethrough;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fontDecoration));
            }
        }
    }
}