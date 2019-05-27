using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Objects.Caching;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRichTextBox : IAmObject
    {
        IntSize2 Size { get; set; }
        float PixelScaling { get; set; }
        [NotNull] IRichText Text { get; set; }
        [CanBeNull] Vector2[] BorderCurve { get;  set; }

        ICacheContainer CacheContainer { get; }
        IRichTextBoxLayout Layout { get; }
    }
}