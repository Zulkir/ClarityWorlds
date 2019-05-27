using System;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Media.Text.Rich;
using Microsoft.Office.Interop.PowerPoint;

namespace Clarity.Ext.Import.Pptx
{
    public static class Converters
    {
        public static RtParagraphAlignment? ToClarity(this PpParagraphAlignment ppAlignment)
        {
            switch (ppAlignment)
            {
                case PpParagraphAlignment.ppAlignmentMixed: 
                    // todo: implement missing alignments
                    return null;
                case PpParagraphAlignment.ppAlignLeft: return RtParagraphAlignment.Left;
                case PpParagraphAlignment.ppAlignCenter: return RtParagraphAlignment.Center;
                case PpParagraphAlignment.ppAlignRight: return RtParagraphAlignment.Right;
                case PpParagraphAlignment.ppAlignJustify: 
                    // todo: implement missing alignments
                    return null;
                case PpParagraphAlignment.ppAlignDistribute: 
                    // todo: implement missing alignments
                    return null;
                case PpParagraphAlignment.ppAlignThaiDistribute: 
                    // todo: implement missing alignments
                    return null;
                case PpParagraphAlignment.ppAlignJustifyLow: 
                    // todo: implement missing alignments
                    return null;
                default: throw new ArgumentOutOfRangeException(nameof(ppAlignment), ppAlignment, null);
            }
        }

        public static RtParagraphDirection? ToClarity(this PpDirection ppDirection)
        {
            switch (ppDirection)
            {
                case PpDirection.ppDirectionMixed: return null;
                case PpDirection.ppDirectionLeftToRight: return RtParagraphDirection.LeftToRight;
                case PpDirection.ppDirectionRightToLeft:return RtParagraphDirection.RightToLeft;
                default: throw new ArgumentOutOfRangeException(nameof(ppDirection), ppDirection, null);
            }
        }

        public static RtListType? ToClarity(this PpBulletType ppBulletType)
        {
            switch (ppBulletType)
            {
                case PpBulletType.ppBulletMixed: return null;
                case PpBulletType.ppBulletNone: return RtListType.None;
                case PpBulletType.ppBulletUnnumbered: return RtListType.Bullets;
                case PpBulletType.ppBulletNumbered: return RtListType.Numbering;
                case PpBulletType.ppBulletPicture: return null;
                default: throw new ArgumentOutOfRangeException(nameof(ppBulletType), ppBulletType, null);
            }
        }

        public static Color4 ToClarity(this ColorFormat ppColorFormat)
        {
            var rgb = ppColorFormat.RGB;
            //var a = 255;// ((argb >> 24) & 0xff) / 255f;
            var b = ((rgb >> 16) & 0xff) / 255f;
            var g = ((rgb >> 8) & 0xff) / 255f;
            var r = (rgb & 0xff) / 255f;
            return new Color4(r, g, b);
        }
    }
}