using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Engine.Media.Text.Rich
{
    public abstract class RtOverallStyle : AmObjectBase<RtOverallStyle>, IRtOverallStyle
    {
        public RichTextDirection Direction { get; set; }
        public abstract Color4 BackgroundColor { get; set; }
        public abstract RtTransparencyMode TransparencyMode { get; set; }
        
        public bool HasTransparency
        {
            get
            {
                switch (TransparencyMode)
                {
                    case RtTransparencyMode.Opaque: return false;
                    case RtTransparencyMode.Native: return BackgroundColor.A < 1;
                    default: return true;
                }
            }
        }
    }
}