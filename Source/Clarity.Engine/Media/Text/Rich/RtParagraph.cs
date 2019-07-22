using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RtParagraph : AmObjectBase<RtParagraph, IAmObject>, IRtParagraph
    {
        public abstract IList<IRtSpan> Spans { get; }
        public abstract IRtParagraphStyle Style { get; set; }

        public int LayoutTextLength => Spans.Sum(x => x.LayoutText.Length);
        public string LayoutText => string.Join("", Spans.Select(x => x.LayoutText));
        public string RawText => string.Join("", Spans.Select(x => x.RawText));
        public string DebugText => string.Join("￥", Spans.Select(x => x.DebugText));

        protected RtParagraph()
        {
            Style = AmFactory.Create<RtParagraphStyle>();
        }

        public void Normalize()
        {
            if (Spans.Count == 0)
                Spans.Add(AmFactory.Create<RtPureSpan>());
            else
                for (var i = Spans.Count - 1; i > 0; i--)
                    if (Spans[i].IsEmpty)
                        Spans.RemoveAt(i);
            // todo: merge spans
        }
    }
}