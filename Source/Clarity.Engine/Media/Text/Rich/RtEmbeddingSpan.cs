using Clarity.Common.Infra.ActiveModel;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RtEmbeddingSpan : AmObjectBase<RtEmbeddingSpan>, IRtEmbeddingSpan
    {
        public abstract IRtSpanStyle Style { get; set; }
        public abstract string EmbeddingType { get; set; }
        public abstract string SourceCode { get; set; }

        public bool IsEmpty => false;
        public int LayoutTextLength => 1;
        public string LayoutText => "☒";
        public string RawText => SourceCode;
        public string DebugText => $"${SourceCode}$";

        protected RtEmbeddingSpan()
        {
            // todo: create default handler
            EmbeddingType = "latex";
            SourceCode = "y=x^2";
        }
    }
}