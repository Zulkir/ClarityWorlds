using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Engine.Media.Text.Rich 
{
    public abstract class DefaultRtListStyle : AmObjectBase<DefaultRtListStyle>, IRtListStyle
    {
        public string GetIconFor(int tabIndex, int numbering)
        {
            switch (tabIndex % 2)
            {
                case 0: return "•  ";
                case 1: return "–  ";
                default: throw new IndexOutOfRangeException();
            }
        }

        public Color4 GetIconColor(Color4 firstSpanColor)
        {
            return firstSpanColor;
        }

        public float GetIconSize(float firstSpanSize)
        {
            return firstSpanSize;
        }
    }
}