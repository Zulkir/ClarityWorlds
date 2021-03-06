﻿using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction.Tools
{
    public class DrawBorderCurveTool : ITool
    {
        private enum State
        {
            Ready,
            Drawing,
            Finished
        }

        private readonly IUndoRedoService undoRedo;
        private readonly ISceneNode entity;
        private readonly IRichTextComponent richTextAspect;
        private readonly IRectangleComponent rectangleAspect;
        private readonly IList<Vector2> curve;
        private State state;

        public DrawBorderCurveTool(ISceneNode entity, IUndoRedoService undoRedo)
        {
            this.undoRedo = undoRedo;
            this.entity = entity;
            richTextAspect = entity.GetComponent<IRichTextComponent>();
            rectangleAspect = entity.GetComponent<IRectangleComponent>();
            richTextAspect.BorderComplete = false;
            richTextAspect.VisualBorderCurve = new List<Vector2>();
            curve = richTextAspect.VisualBorderCurve;
            state = State.Ready;
        }

        public bool TryHandleInputEvent(IInputEvent eventArgs)
        {
            return eventArgs is IMouseEvent mouseArgs && TryHandleMouseEvent(mouseArgs);
        }

        private bool TryHandleMouseEvent(IMouseEvent eventArgs)
        {
            if (state == State.Ready && eventArgs.IsLeftDownEvent())
            {
                state = State.Drawing;
                AddPointIfPossible(eventArgs.Viewport, eventArgs.State.Position);
                return true;
            }
            if (state == State.Drawing && eventArgs.IsOfType(MouseEventType.Move))
            {
                AddPointIfPossible(eventArgs.Viewport, eventArgs.State.Position);
                return true;
            }
            if (state == State.Drawing && eventArgs.IsLeftUpEvent())
            {
                AddPointIfPossible(eventArgs.Viewport, eventArgs.State.Position);
                AdjustRectangleAndSetCurve();
                richTextAspect.BorderComplete = true;
                state = State.Finished;
                return true;
            }
            return false;
        }

        private void AddPointIfPossible(IViewport viewport, IntVector2 pixelPos)
        {
            if (TryGetPoint(viewport, pixelPos, out var point))
                curve.Add(point);
        }

        private bool TryGetPoint(IViewport viewport, IntVector2 pixelPos, out Vector2 point)
        {
            point = default(Vector2);
            // todo: search for a placement plane in indirect parents
            var cPlacement = entity.ParentNode?.PresentationInfra().Placement;
            if (cPlacement == null)
                return false;
            var globalRay = viewport.GetGlobalRayForPixelPos(pixelPos);
            if (!cPlacement.PlacementSurface2D.TryFindPoint2D(globalRay, out var layoutPoint))
                return false;
            var entityRect = rectangleAspect.Rectangle;
            point = layoutPoint - entityRect.Center;
            point.X /= entityRect.HalfWidth;
            point.Y /= entityRect.HalfHeight;
            return true;
        }

        private void AdjustRectangleAndSetCurve()
        {
            var entityRect = rectangleAspect.Rectangle;
            var layoutBasedPoints = curve
                .Select(xy => entityRect.Center + new Vector2(xy.X * entityRect.HalfWidth, xy.Y * entityRect.HalfHeight))
                .ToArray();
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            foreach (var point in layoutBasedPoints)
            {
                if (point.X < minX)
                    minX = point.X;
                if (point.X > maxX)
                    maxX = point.X;
                if (point.Y < minY)
                    minY = point.Y;
                if (point.Y > maxY)
                    maxY = point.Y;
            }
            var newRect = new AaRectangle2(new Vector2(minX, minY), new Vector2(maxX, maxY));
            var adjustedCurve = layoutBasedPoints.Select(xy =>
            {
                var result = xy - newRect.Center;
                result.X /= newRect.HalfWidth;
                result.Y /= newRect.HalfHeight;
                return result;
            }).ToArray();
            rectangleAspect.Rectangle = newRect;
            richTextAspect.BorderCurve = adjustedCurve;
            richTextAspect.VisualBorderCurve = adjustedCurve;
            undoRedo.OnChange();
        }

        public void Dispose()
        {
            if (state != State.Finished)
                richTextAspect.VisualBorderCurve = null;
        }
    }
}