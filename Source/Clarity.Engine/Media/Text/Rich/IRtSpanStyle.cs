using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtSpanStyle : IAmObject, IEquatable<IRtSpanStyle>
    {
        [NotNull]
        string FontFamily { get; set; }
        FontDecoration FontDecoration { get; set; }
        float Size { get; set; }
        Color4 TextColor { get; set; }
        [CanBeNull]
        string HighlightGroup { get; set; }

        void CopyFrom(IRtSpanStyle other);
    }
}