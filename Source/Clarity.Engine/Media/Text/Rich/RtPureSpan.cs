using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RtPureSpan : AmObjectBase<RtPureSpan>, IRtPureSpan
    {
        public abstract string Text { get; set; }
        public abstract IRtSpanStyle Style { get; set; }

        public int LayoutTextLength => LayoutText.Length;
        public string LayoutText => Text;
        public string RawText => Text;
        public string DebugText => Text;
        public bool IsEmpty => Text.Length == 0;

        protected RtPureSpan()
        {
            Text = "";
            Style = AmFactory.Create<RtSpanStyle>();
        }
    }
}