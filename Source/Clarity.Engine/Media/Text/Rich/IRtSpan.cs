using Clarity.Common.Infra.ActiveModel;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtSpan : IAmObject
    {
        [NotNull]
        string Text { get; set; }
        [NotNull]
        IRtSpanStyle Style { get; set; }

        int Length { get; }
        bool IsEmpty { get; }
    }
}