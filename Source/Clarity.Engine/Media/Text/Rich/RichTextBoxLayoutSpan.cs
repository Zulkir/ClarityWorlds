using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using JetBrains.Annotations;

namespace Clarity.Engine.Media.Text.Rich
{
    public struct RichTextBoxLayoutSpan
    {
        public AaRectangle2 Bounds;
        public AaRectangle2 Strip;
        public IReadOnlyList<float> CharOffsets;
        public RtPosition TextPosition;
        public string Text;
        public IRtSpanStyle Style;
        [CanBeNull]
        public IRtEmbeddingSpan Embedding;
        [CanBeNull]
        public IRtEmbeddingHandler EmbeddingHandler;
        [CanBeNull]
        public IImage EmbeddingImage;

        public float GetCharOffset(int charIndex) => charIndex < CharOffsets.Count ? CharOffsets[charIndex] : Bounds.Width;

        public float DistanceFrom(Vector2 point)
        {
            var relPoint = point - Bounds.Center;
            var absRelPoint = new Vector2(Math.Abs(relPoint.X), Math.Abs(relPoint.Y));
            var axisDistances = new Vector2(
                Math.Max(0, absRelPoint.X - Bounds.HalfWidth),
                Math.Max(0, absRelPoint.Y - Bounds.HalfHeight));
            return axisDistances.X + axisDistances.Y * 100000;
        }

        public int ClosestIndexFor(float pointX)
        {
            if (Text.Length == 0)
                return 0;
            var rect = Bounds;
            return CharOffsets
                .Select((x, i) => Tuples.Pair(i, x))
                .ConcatSingle(Tuples.Pair(CharOffsets.Count, rect.MaxX))
                .Minimal(x => Math.Abs(pointX - (rect.MinX + x.Second)))
                .First;
        }
    }
}