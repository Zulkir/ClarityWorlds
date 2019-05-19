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

        int Length { get; }
        bool IsEmpty { get; }

        string RawText { get; }

        void Normalize();
    }
}