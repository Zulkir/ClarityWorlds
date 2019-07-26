using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Common;

namespace Clarity.Engine.Media.Text.Rich
{
    public class RichTextBoxLayoutBuilder : IRichTextBoxLayoutBuilder
    {
        private class BuildingContext
        {
            public IRichText Text;
            public List<RichTextBoxLayoutSpan> LayoutSpans;
            public List<RichTextBoxLayoutSpan> ExternalLayoutSpans;
            public Vector2 StripStartPoint;
            // todo: to a flexible shape
            public AaRectangle2 RemainingShape;
        }

        private struct Subspan
        {
            public RtPosition TextRelPosition;
            public string Text;
            public IRtSpanStyle Style;
            public IRtEmbeddingSpan Embedding;
            public IRtEmbeddingHandler EmbeddingHandler;
            public IImage EmbeddingImage;
            public RichTextDirection TextDirection;
            public RtParagraphDirection ParagraphDirection;
            public bool CanBreakAfter;

        }

        private readonly IRichTextMeasurer measurer;
        private readonly ITextLineBreaker lineBreaker;
        private readonly IRtEmbeddingHandlerContainer embeddingHandlerContainer;

        public RichTextBoxLayoutBuilder(IRichTextMeasurer measurer, ITextLineBreaker lineBreaker, IRtEmbeddingHandlerContainer embeddingHandlerContainer)
        {
            this.measurer = measurer;
            this.lineBreaker = lineBreaker;
            this.embeddingHandlerContainer = embeddingHandlerContainer;
        }

        public IRichTextBoxLayout Build(IRichText text, IntSize2 size)
        {
            text.Normalize();
            var lspans = new List<RichTextBoxLayoutSpan>();
            var externalLayoutSpans = new List<RichTextBoxLayoutSpan>();
            var context = new BuildingContext
            {
                Text = text,
                LayoutSpans = lspans,
                ExternalLayoutSpans = externalLayoutSpans,
                RemainingShape = new AaRectangle2(Vector2.Zero, new Vector2(size.Width, size.Height))
            };

            for (var i = 0; i < text.Paragraphs.Count; i++)
                BuildParagraph(context, i);

            if (lspans.Count == 0)
            {
                var defaultStyle = text.Paragraphs[0].Spans[0].Style;
                lspans.Add(new RichTextBoxLayoutSpan
                {
                    TextAbsPosition = 0,
                    TextRelPosition = new RtPosition(),
                    Bounds = new AaRectangle2(new Vector2(0, 0), new Vector2(1f, defaultStyle.Size)),
                    Strip = new AaRectangle2(new Vector2(0, 0), new Vector2(1f, defaultStyle.Size)),
                    Style = defaultStyle,
                    Text = "",
                    CharOffsets = EmptyArrays<float>.Array
                });
            }

            return new RichTextBoxLayout(text, lspans, externalLayoutSpans);
        }

        private void BuildParagraph(BuildingContext context, int paragraphIndex)
        {
            var para = context.Text.Paragraphs[paragraphIndex];
            ApplyMarginUp(context, para);

            var subspans = BuildParagraphSubspans(context, paragraphIndex);

            var flushedSubspanCount = 0;
            while (flushedSubspanCount < subspans.Count)
            {
                var minimalSubspans = new SubList<Subspan>(subspans, flushedSubspanCount, 0);

                for (var i = flushedSubspanCount; i < subspans.Count; i++)
                {
                    minimalSubspans = minimalSubspans.WithNext();
                    if (subspans[i].CanBreakAfter)
                        break;
                }

                if (!TryAllocateNewStrip(context, minimalSubspans))
                    return;

                var subspansToFlush = minimalSubspans;
                var subspansToTry = subspansToFlush.WithNext();
                var tabOffset = context.Text.Paragraphs[paragraphIndex].Style.TabCount * 80f;
                while (flushedSubspanCount + subspansToFlush.Count < subspans.Count && CanFitSubspans(context, subspansToTry, tabOffset))
                {
                    subspansToFlush = subspansToTry;
                    subspansToTry = subspansToFlush.WithNext();
                }

                var isFirstStrip = flushedSubspanCount == 0;
                var isLastStrip = flushedSubspanCount + subspansToFlush.Count == subspans.Count;
                FlushStrip(context, paragraphIndex, tabOffset, subspansToFlush, isFirstStrip, isLastStrip);
                flushedSubspanCount += subspansToFlush.Count;
            }
        }

        private void ApplyMarginUp(BuildingContext context, IRtParagraph para)
        {
            var marginUp = para.Style.MarginUp;
            var oldShape = context.RemainingShape;
            context.StripStartPoint.Y += marginUp;
            context.RemainingShape = new AaRectangle2(oldShape.MinMin + new Vector2(0, marginUp), oldShape.MaxMax);
        }

        private IReadOnlyList<Subspan> BuildParagraphSubspans(BuildingContext context, int paragraphIndex)
        {
            var subspans = new List<Subspan>();
            var text = context.Text;
            var para = text.Paragraphs[paragraphIndex];
            para.Normalize();
            var firstSpanStyle = para.Spans.First().Style;
            /*
            if (paragraph.Style.ListType != RtListType.None)
            {
                var tabIndex = paragraph.Style.TabCount;
                var numbering = GetParagraphNumbering(text, paragraphIndex);
                var icon = paragraph.Style.ListStyle.GetIconFor(tabIndex, numbering);
                var iconStyle = firstSpanStyle.CloneTyped();
                iconStyle.Size = paragraph.Style.ListStyle.GetIconSize(firstSpanStyle.Size);
                iconStyle.TextColor = paragraph.Style.ListStyle.GetIconColor(firstSpanStyle.TextColor);
                subspans.Add(new Subspan
                {
                    TextRelPosition = new RtPosition(paragraphIndex, 0, 0),
                    Text = icon,
                    Style = iconStyle,
                    ParagraphDirection = paragraph.Style.Direction,
                    TextDirection = text.Style.Direction,
                    CanBreakAfter = false
                });
                subspans.Add(new Subspan
                {
                    TextRelPosition = new RtPosition(paragraphIndex, 0, 0),
                    Text = new string('\t', tabIndex + 1),
                    Style = iconStyle,
                    ParagraphDirection = paragraph.Style.Direction,
                    TextDirection = text.Style.Direction,
                    CanBreakAfter = false
                });
            }*/

            var paraLayoutText = para.LayoutText;
            var paraPos = 0;
            for (var spanIndex = 0; spanIndex < para.Spans.Count; spanIndex++)
            {
                var span = para.Spans[spanIndex];
                switch (span)
                {
                    case IRtEmbeddingSpan embeddingSpan:
                    {
                        var handler = embeddingHandlerContainer.GetHandler(embeddingSpan);
                        var image = handler.BuildImage(embeddingSpan);
                        subspans.Add(new Subspan
                        {
                            TextRelPosition = new RtPosition(paragraphIndex, spanIndex, 0),
                            Text = "☒",
                            Style = span.Style,
                            CanBreakAfter = true,
                            ParagraphDirection = para.Style.Direction,
                            TextDirection = text.Style.Direction,
                            Embedding = embeddingSpan,
                            EmbeddingHandler = handler,
                            EmbeddingImage = image
                        });
                        break;
                    }
                    case IRtPureSpan pureSpan:
                    {
                        if (pureSpan.Text.Length == 0)
                            continue;

                        var previousBreakCharIndex = 0;
                        for (var charIndex = 1; charIndex < pureSpan.Text.Length; charIndex++)
                        {
                            if (lineBreaker.CanBreakAt(paraLayoutText, paraPos + charIndex))
                            {
                                subspans.Add(new Subspan
                                {
                                    TextRelPosition = new RtPosition(paragraphIndex, spanIndex, previousBreakCharIndex),
                                    Text = pureSpan.Text.Substring(previousBreakCharIndex, charIndex - previousBreakCharIndex),
                                    Style = span.Style,
                                    // todo: consider text direction
                                    CanBreakAfter = true,
                                    ParagraphDirection = para.Style.Direction,
                                    TextDirection = text.Style.Direction
                                });
                                previousBreakCharIndex = charIndex;
                            }
                        }

                        subspans.Add(new Subspan
                        {
                            TextRelPosition = new RtPosition(paragraphIndex, spanIndex, previousBreakCharIndex),
                            Text = pureSpan.Text.Substring(previousBreakCharIndex),
                            Style = span.Style,
                            // todo: consider text direction
                            CanBreakAfter = true,
                            ParagraphDirection = para.Style.Direction,
                            TextDirection = text.Style.Direction
                        });
                        break;
                    }
                }

                paraPos += span.LayoutTextLength;
            }

            if (subspans.Count == 0)
            {
                subspans.Add(new Subspan
                {
                    TextRelPosition = new RtPosition(paragraphIndex, 0, 0),
                    Text = "",
                    Style = firstSpanStyle,
                    ParagraphDirection = para.Style.Direction,
                    TextDirection = text.Style.Direction,
                    CanBreakAfter = false
                });
            }

            return subspans;
        }

        private static int GetParagraphNumbering(IRichText text, int paragraphIndex)
        {
            if (text.Paragraphs[paragraphIndex].Style.ListType != RtListType.Numbering)
                return 0;
            if (paragraphIndex == 0)
                return 1;
            return GetParagraphNumbering(text, paragraphIndex - 1) + 1;
        }

        private int? SearchNextBreakingPosition(string paragraphRawText, int prevPosition)
        {
            var pos = prevPosition + 1;
            while (pos < paragraphRawText.Length)
                if (lineBreaker.CanBreakAt(paragraphRawText, pos))
                    return pos;
            return null;
        }

        private bool TryAllocateNewStrip(BuildingContext context, SubList<Subspan> firstAtomicSubspans)
        {
            var firstSize = MeasureSubspans(context, firstAtomicSubspans);
            // todo: consider directions
            context.StripStartPoint = context.RemainingShape.MinMin;
            return firstSize.Width <= context.RemainingShape.Width &&
                   firstSize.Height <= context.RemainingShape.Height;
        }

        private bool CanFitSubspans(BuildingContext context, SubList<Subspan> subspans, float tabOffset)
        {
            var size = MeasureSubspans(context, subspans);
            var availableWidth = context.RemainingShape.Width;
            var availableHeight = context.RemainingShape.Height;
            return size.Width <= availableWidth - tabOffset && 
                   size.Height <= availableHeight;
        }

        private void FlushStrip(BuildingContext context, int paragraphIndex, float tabOffset, SubList<Subspan> subspans, bool isFirstStrip, bool isLastStrip)
        {
            // todo: consider direction
            var paragraph = context.Text.Paragraphs[paragraphIndex];
            //if (!isLastStrip)
            //    subspans = TrimEnd(subspans);
            var commonSize = MeasureSubspans(context, subspans);
            var stripSize = new Size2(context.RemainingShape.Width, commonSize.Height);
            if (paragraph.Style.Alignment != RtParagraphAlignment.Justify || isLastStrip)
            {
                // todo: tabs
                var mergedSubspans = MergeSubspans(subspans);
                var newRectangles = new RichTextBoxLayoutSpan[mergedSubspans.Count];
                var strip = AaRectangle2.FromCornerAndDimensions(
                    context.StripStartPoint.X, context.StripStartPoint.Y, 
                    commonSize.Width, commonSize.Height);
                var rectOffsetX = tabOffset;
                var charIndex = 0;

                for (var i = 0; i < mergedSubspans.Count; i++)
                {
                    var subspan = mergedSubspans[i];
                    var subspanSize = MeasureSubspan(subspan);

                    var charWidths = subspan.Text.Select(x => measurer.GetCharSize(x, subspan.Style).Width).ToArray();
                    var subspanWidthWithoutKerning = charWidths.Sum();
                    var kerningAdjustment = subspanSize.Width / subspanWidthWithoutKerning;
                    var adjustedCharWidths = charWidths.Select(x => x * kerningAdjustment).ToArray();
                    var charOffsets = Enumerable.Range(0, subspan.Text.Length).Select(x => adjustedCharWidths.Take(x).Sum()).ToArray();
                    
                    newRectangles[i] = new RichTextBoxLayoutSpan
                    {
                        TextRelPosition = subspan.TextRelPosition,
                        TextAbsPosition = context.Text.GetGlobalIndex(subspan.TextRelPosition),
                        Bounds = new AaRectangle2(
                            new Vector2(rectOffsetX, context.StripStartPoint.Y),
                            new Vector2(rectOffsetX + subspanSize.Width, context.StripStartPoint.Y + commonSize.Height)),
                        Strip = strip,
                        CharOffsets = charOffsets,
                        Text = subspan.Text,
                        Style = subspan.Style,
                        Embedding = subspan.Embedding,
                        EmbeddingHandler = subspan.EmbeddingHandler,
                        EmbeddingImage = subspan.EmbeddingImage
                    };

                    rectOffsetX += subspanSize.Width;
                    charIndex += subspan.Text.Length;
                }

                var adjustment = 0f;
                switch (paragraph.Style.Alignment)
                {
                    case RtParagraphAlignment.Left:
                        break;
                    case RtParagraphAlignment.Center:
                        var leftCenterX = rectOffsetX / 2;
                        var stripCenter = stripSize.Width / 2;
                        adjustment = stripCenter - leftCenterX;
                        break;
                    case RtParagraphAlignment.Right:
                        adjustment = stripSize.Width - rectOffsetX;
                        break;
                    case RtParagraphAlignment.Justify:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                for (var i = 0; i < newRectangles.Length; i++)
                    newRectangles[i].Bounds.Center.X += adjustment;
                context.LayoutSpans.AddRange(newRectangles);
                context.RemainingShape = new AaRectangle2(context.RemainingShape.MinMin + Vector2.UnitY * stripSize.Height, 
                                                          context.RemainingShape.MaxMax);

                if (isFirstStrip)
                {
                    // todo: adjust for other directions
                    var bulletStr = paragraph.Style.ListStyle.GetIconFor(paragraph.Style.TabCount, /*todo*/0);
                    FlushBullet(context, new Vector2(tabOffset, strip.MaxY), bulletStr, subspans.First().Style);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void FlushBullet(BuildingContext context, Vector2 firstSpanPoint, string bulletStr, IRtSpanStyle style)
        {
            var size = measurer.MeasureString(bulletStr, style);
            var otherCorner = firstSpanPoint - size.ToVector();
            context.ExternalLayoutSpans.Add(new RichTextBoxLayoutSpan
            {
                Text = bulletStr,
                Bounds = new AaRectangle2(otherCorner, firstSpanPoint),
                Style = style
            });
        }

        private static IReadOnlyList<Subspan> MergeSubspans(SubList<Subspan> subspans)
        {
            var resultList = new List<Subspan>();
            var toMerge = new List<Subspan>();
            var index = 0;
            while (index < subspans.Count)
            {
                var first = subspans[index];
                index++;

                if (first.EmbeddingImage != null)
                {
                    resultList.Add(first);
                    continue;
                }

                toMerge.Clear();
                toMerge.Add(first);
                
                // todo: consider span direction
                while (index < subspans.Count && subspans[index].TextRelPosition.SpanIndex == toMerge[0].TextRelPosition.SpanIndex)
                {
                    toMerge.Add(subspans[index]);
                    index++;
                }
                resultList.Add(new Subspan
                {
                    TextRelPosition = first.TextRelPosition,
                    Text = toMerge.Select(x => x.Text).Aggregate((x, y) => x + y),
                    Style = first.Style,
                    CanBreakAfter = toMerge.Last().CanBreakAfter,
                    ParagraphDirection = first.ParagraphDirection,
                    TextDirection = first.TextDirection
                });
            }
            return resultList;
        }

        private Size2 MeasureSubspans(BuildingContext context, SubList<Subspan> subspans)
        {
            // todo: cache measurments for a context
            // todo: consider directions
            subspans = TrimEnd(subspans);
            var sizes = subspans.Select(MeasureSubspan).ToArray();
            return new Size2(sizes.Sum(x => x.Width), sizes.Max(x => x.Height));
        }

        private Size2 MeasureSubspan(Subspan subspan)
        {
            if (subspan.EmbeddingImage != null)
            {
                var intSize = subspan.EmbeddingImage.Size.Width;
                return new Size2(intSize, subspan.EmbeddingImage.Size.Height);
            }
            if (subspan.Text.Length == 0)
            {
                var size = measurer.MeasureString(" ", subspan.Style);
                return new Size2(0, size.Height);
            }
            return measurer.MeasureString(subspan.Text, subspan.Style);
        }

        private static SubList<Subspan> TrimEnd(SubList<Subspan> subspans)
        {
            while (subspans.Count > 1 && CanTrim(subspans.Last()))
                subspans = subspans.WithoutLast();
            return subspans;
        }

        private static bool CanTrim(Subspan subspan)
        {
            return subspan.EmbeddingImage == null && subspan.Text.All(y => y == ' ' || y == '　');
        }
    }
}