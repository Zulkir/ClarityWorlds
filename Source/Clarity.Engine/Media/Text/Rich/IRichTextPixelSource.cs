using Clarity.Engine.Media.Images;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichTextPixelSource : IPixelSource
    {
        [NotNull]
        IRichTextBox TextBox { get; }
    }
}