using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.Utilities;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RtParagraphStyle : AmObjectBase<RtParagraphStyle>, IRtParagraphStyle
    {
        public abstract RtParagraphAlignment Alignment { get; set; }
        public abstract RtParagraphDirection Direction { get; set; }
        public abstract RtListType ListType { get; set; }
        public abstract IRtListStyle ListStyle { get; set; }
        public abstract int TabCount { get; set; }
        public abstract float MarginUp { get; set; }

        protected RtParagraphStyle()
        {
            ListStyle = AmFactory.Create<DefaultRtListStyle>();
        }
    }
}