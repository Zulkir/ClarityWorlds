using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtListStyle : IAmObject
    {
        string GetIconFor(int tabIndex, int numbering);
        Color4 GetIconColor(Color4 firstSpanColor);
        float GetIconSize(float firstSpanSize);
    }
}