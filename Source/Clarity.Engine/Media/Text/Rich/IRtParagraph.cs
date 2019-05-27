using System.Collections.Generic;
using Clarity.Common.Infra.ActiveModel;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtParagraph : IAmObject
    {
        [NotNull]
        IList<IRtSpan> Spans { get; }
        [NotNull]
        IRtParagraphStyle Style { get; set; }

        int LayoutTextLength { get; }

        string LayoutText { get; }
        string RawText { get; }
        string DebugText { get; }

        void Normalize();
    }
}