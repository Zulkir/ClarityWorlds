using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public static class RichTextHelper
    {
        public static IRichTextBox Label(string text, 
            IntSize2 size, RtParagraphAlignment alignment, Color4 backgroundColor, RtTransparencyMode transparencyMode,
            string fontFamily, float fontSize, Color4 fontColor, FontDecoration fontDecoration)
        {
            var spanStyle = AmFactory.Create<RtSpanStyle>();
            spanStyle.Size = fontSize;
            spanStyle.FontFamily = fontFamily;
            spanStyle.FontDecoration = fontDecoration;
            spanStyle.TextColor = fontColor;
            var span = AmFactory.Create<RtSpan>();
            span.Text = text;
            span.Style = spanStyle;

            var paraStyle = AmFactory.Create<RtParagraphStyle>();
            paraStyle.Alignment = alignment;
            var para = AmFactory.Create<RtParagraph>();
            para.Spans.Add(span);
            para.Style = paraStyle;

            var textStyle = AmFactory.Create<RtOverallStyle>();
            textStyle.BackgroundColor = backgroundColor;
            textStyle.TransparencyMode = transparencyMode;
            var richText = AmFactory.Create<RichText>();
            richText.Paragraphs.Add(para);
            richText.Style = textStyle;

            var textBox = AmFactory.Create<RichTextBox>();
            textBox.Size = size;
            textBox.Text = richText;

            return textBox;
        }
    }
}