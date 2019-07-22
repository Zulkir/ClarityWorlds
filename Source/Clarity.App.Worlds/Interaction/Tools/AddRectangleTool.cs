using System;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.App.Worlds.UndoRedo;
using Clarity.App.Worlds.Views;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction.Tools
{
    public class AddRectangleTool : ITool
    {
        private enum State
        {
            Ready,
            Began,
            Expanding,
            Finished
        }

        private readonly IToolService toolService;
        private readonly IUndoRedoService undoRedo;
        private readonly ISceneNode entity;
        private readonly IRectangleComponent rectangleComponent;

        private readonly bool preserveAspectRatio;
        private readonly float aspectRatio;

        private State state;
        private Vector2 firstPoint;

        public AddRectangleTool(IToolService toolService, IUndoRedoService undoRedo, 
            ICommonNodeFactory commonNodeFactory, IAmDiBasedObjectFactory objectFactory, IImage image, IMovie movie, bool text)
        {
            this.toolService = toolService;
            this.undoRedo = undoRedo;
            if (text)
                entity = commonNodeFactory.RichTextRectangle(objectFactory.Create<RichText>());
            else if (movie != null)
                entity = commonNodeFactory.MovieRectangleNode(movie);
            else if (image != null)
                entity = commonNodeFactory.ImageRectangleNode(image);
            else
                entity = commonNodeFactory.ColorRectangleNode(GetRandomSaturatedColor());
            rectangleComponent = entity.GetComponent<IRectangleComponent>();

            if (movie != null)
            {
                aspectRatio = (float)movie.Width / Math.Max(movie.Height, 1);
                preserveAspectRatio = true;
            }
            else if (image != null)
            {
                aspectRatio = (float)image.Size.Width / Math.Max(image.Size.Height, 1);
                preserveAspectRatio = true;
            }

            state = State.Ready;
        }

        public bool TryHandleInputEvent(IInputEvent eventArgs)
        {
            return eventArgs is IMouseEvent mouseArgs && TryHandleMouseEvent(mouseArgs);
        }

        private bool TryHandleMouseEvent(IMouseEvent eventArgs)
        {
            void AdjustForAspectRatio(ref Vector2 sp)
            {
                if (!preserveAspectRatio)
                    return;
                sp.X = firstPoint.X > sp.X
                    ? firstPoint.X - aspectRatio * Math.Abs(firstPoint.Y - sp.Y)
                    : firstPoint.X + aspectRatio * Math.Abs(firstPoint.Y - sp.Y);
            }

            switch (state)
            {
                case State.Ready:
                    if (eventArgs.EventButtons == MouseButtons.Left)
                    {
                        if (!TryGetPoint(eventArgs, out _, out firstPoint))
                        {
                            entity.Deparent();
                            return false;
                        }
                        state = State.Began;
                    }
                    return true;
                case State.Began:
                    if ((eventArgs.State.Buttons & MouseButtons.Left) == 0)
                    {
                        state = State.Ready;
                    }
                    else if (eventArgs.IsOfType(MouseEventType.Move))
                    {
                        if (!TryGetPoint(eventArgs, out var placementNode, out var secondPoint))
                        {
                            entity.Deparent();
                            return false;
                        }
                        AdjustForAspectRatio(ref secondPoint);
                        rectangleComponent.Rectangle = new AaRectangle2(firstPoint, secondPoint);
                        placementNode.ChildNodes.AddUnique(entity);
                        state = State.Expanding;
                    }
                    return true;
                case State.Expanding:
                    if ((eventArgs.State.Buttons & MouseButtons.Left) == 0)
                    {
                        if (!TryGetPoint(eventArgs, out var placementNode, out var secondPoint))
                        {
                            entity.Deparent();
                            state = State.Ready;
                            return false;
                        }
                        AdjustForAspectRatio(ref secondPoint);
                        rectangleComponent.Rectangle = new AaRectangle2(firstPoint, secondPoint);
                        entity.Deparent();
                        placementNode.ChildNodes.Add(entity);
                        undoRedo.OnChange();
                        state = State.Finished;
                        toolService.CurrentTool = null;
                    }
                    else if (eventArgs.IsOfType(MouseEventType.Move))
                    {
                        if (!TryGetPoint(eventArgs, out _, out var secondPoint))
                        {
                            entity.Deparent();
                            return false;
                        }
                        AdjustForAspectRatio(ref secondPoint);
                        rectangleComponent.Rectangle = new AaRectangle2(firstPoint, secondPoint);
                    }
                    return true;
                case State.Finished:
                    throw new InvalidOperationException("Tryng to reuse a tool.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool TryGetPoint(IMouseEvent mouseEvent, out ISceneNode placementNode, out Vector2 point)
        {
            point = default(Vector2);

            var viewport = mouseEvent.Viewport;
            placementNode = (viewport.View as IFocusableView)?.FocusNode;
            if (placementNode == null)
                return false;
            var cPlacement = placementNode.SearchComponent<IPlacementComponent>();
            if (cPlacement == null)
                return false;
            var globalRay = viewport.GetGlobalRayForPixelPos(mouseEvent.State.Position);
            return cPlacement.PlacementSurface2D.TryFindPoint2D(globalRay, out point);
        }

        private static Color4 GetRandomSaturatedColor()
        {
            var rand = new Random();
            int type = rand.Next(0, 5);
            switch (type)
            {
                default:
                case 0: return new Color4(0f, 1f, (float)rand.NextDouble());
                case 1: return new Color4(0f, (float)rand.NextDouble(), 1f);
                case 2: return new Color4((float)rand.NextDouble(), 0f, 1f);
                case 3: return new Color4(1f, 0f, (float)rand.NextDouble());
                case 4: return new Color4(1f, (float)rand.NextDouble(), 0f);
                case 5: return new Color4((float)rand.NextDouble(), 1f, 0f);
            }
        }

        public void Dispose()
        {
            if (state != State.Finished)
                entity.Deparent();
        }
        /*
        private static readonly string LoremIpsum =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, " +
            "sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
            //"Ut enim ad minim veniam, " +
            //"quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
            //"Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
            //"Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
            ;*/
    }
}