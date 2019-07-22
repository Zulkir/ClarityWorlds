using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtPureSpan : IRtSpan
    {
        [NotNull]
        string Text { get; set; }
        bool IsEmpty { get; }
    }
}