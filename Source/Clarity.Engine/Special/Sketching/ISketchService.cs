using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Interaction.Input.Mouse;

namespace Clarity.Engine.Special.Sketching
{
    // todo: remove and replace with visual elements in a tree
    public interface ISketchService
    {
        IReadOnlyList<IReadOnlyList<Vector2>> GetSketches();
        int GetTotalPointCount();
        bool TryHandleMouseEvent(IMouseEventArgs args);
    }
}