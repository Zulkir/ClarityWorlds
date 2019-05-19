using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;

namespace Clarity.Engine.Special.Sketching
{
    public class SketchService : ISketchService
    {
        private readonly List<List<Vector2>> sketches;
        private bool isDrawing;

        public SketchService()
        {
            sketches = new List<List<Vector2>>();
        }

        public IReadOnlyList<IReadOnlyList<Vector2>> GetSketches() => sketches;
        public int GetTotalPointCount() => sketches.Sum(x => x.Count);

        public bool TryHandleMouseEvent(IMouseEventArgs args)
        {
            if (!isDrawing)
            {
                if (args.IsLeftDownEvent() && args.KeyModifyers == KeyModifyers.Shift)
                {
                    isDrawing = true;
                    var newSketch = new List<Vector2>();
                    newSketch.Add(args.State.HmgnPosition);
                    sketches.Add(newSketch);
                    return true;
                }
                if (args.IsRightClickEvent() && args.KeyModifyers == KeyModifyers.Shift)
                {
                    if (sketches.Any())
                        sketches.RemoveAt(sketches.Count - 1);
                    return true;
                }
            }
            else
            {
                if (args.IsLeftUpEvent() || args.KeyModifyers != KeyModifyers.Shift)
                {
                    sketches.Last().Add(args.State.HmgnPosition);
                    isDrawing = false;
                    return true;
                }
                if (args.IsOfType(MouseEventType.Move))
                {
                    sketches.Last().Add(args.State.HmgnPosition);
                    return true;
                }
            }
            return false;
        }
    }
}