using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RtSpan : AmObjectBase<RtSpan>, IRtSpan
    {
        public abstract string Text { get; set; }
        public abstract IRtSpanStyle Style { get; set; }

        public int Length => Text.Length;
        public bool IsEmpty => Text.Length == 0;

        protected RtSpan()
        {
            Text = "";
            Style = AmFactory.Create<RtSpanStyle>();
        }
    }
}