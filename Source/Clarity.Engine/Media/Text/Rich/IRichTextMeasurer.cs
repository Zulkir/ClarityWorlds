using Clarity.Common.Numericals.Geometry;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichTextMeasurer
    {
        Size2 MeasureString(string str, IRtSpanStyle style);
        Size2 GetCharSize(char ch, IRtSpanStyle style);
    }
}