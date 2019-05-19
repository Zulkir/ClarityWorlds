using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtOverallStyle : IAmObject
    {
        RichTextDirection Direction { get; set; }
        Color4 BackgroundColor { get; set; }
        RtTransparencyMode TransparencyMode { get; set; }
        bool HasTransparency { get; }
    }
}