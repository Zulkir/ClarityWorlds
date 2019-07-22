using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RtSpanStyle : AmObjectBase<RtSpanStyle>, IRtSpanStyle
    {
        public abstract string FontFamily { get; set; }
        public abstract FontDecoration FontDecoration { get; set; }
        public abstract float Size { get; set; }
        public abstract Color4 TextColor { get; set; }
        public abstract string HighlightGroup { get; set; }

        protected RtSpanStyle()
        {
            FontFamily = "Arial";
            Size = 30f;
            TextColor = Color4.Black;
            FontDecoration = FontDecoration.Bold;
        }

        public bool Equals(IRtSpanStyle other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualsNotNull(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!(obj is IRtSpanStyle other)) return false;
            return Equals(other);
        }

        private bool EqualsNotNull([NotNull] IRtSpanStyle other)
        {
            return
                FontFamily == other.FontFamily &&
                FontDecoration == other.FontDecoration &&
                Size == other.Size &&
                TextColor == other.TextColor;
        }

        public override int GetHashCode()
        {
            return FontFamily.GetHashCode() ^ FontDecoration.GetHashCode() ^ Size.GetHashCode() ^ TextColor.GetHashCode();
        }
    }
}