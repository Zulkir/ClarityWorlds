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

        public int Length => Spans.Sum(x => x.Text.Length);
        public bool IsEmpty => Spans.All(x => x.IsEmpty);
        public string RawText => Spans.Select(x => x.Text).Aggregate((x, y) => x + y);

        protected RtParagraph()
        {
            Style = AmFactory.Create<RtParagraphStyle>();
        }

        public string DebugText => string.Join("￥", Spans.Select(x => x.Text));

        public void Normalize()
        {
            if (Spans.Count == 0)
                Spans.Add(AmFactory.Create<RtSpan>());
            else
                for (var i = Spans.Count - 1; i > 0; i--)
                    if (Spans[i].IsEmpty)
                        Spans.RemoveAt(i);
            // todo: merge spans
        }
    }
}