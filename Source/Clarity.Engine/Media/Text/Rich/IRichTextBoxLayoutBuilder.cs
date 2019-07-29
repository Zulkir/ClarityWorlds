using Clarity.Common.Numericals.Geometry;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichTextBoxLayoutBuilder
    {
        IRichTextBoxLayout Build(IRichText text, IntSize2 size, float padding);
    }
}