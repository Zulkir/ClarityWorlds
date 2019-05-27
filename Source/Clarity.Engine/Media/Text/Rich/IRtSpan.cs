using Clarity.Common.Infra.ActiveModel;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtSpan : IAmObject
    {
        [NotNull]
        IRtSpanStyle Style { get; }

        bool IsEmpty { get; }

        int LayoutTextLength { get; }
        string LayoutText { get; }
        string RawText { get; }
        string DebugText { get; }
    }
}