using System;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Media.Media2D;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction
{
    public class MoviePlaybackInteractionElement<TMaster> : IInteractionElement
        where TMaster : ISceneNodeBound
    {
        private readonly TMaster master;
        private readonly Func<TMaster, IMoviePlayback> getPlayback;

        public MoviePlaybackInteractionElement(TMaster master,
            Func<TMaster, IMoviePlayback> getPlayback)
        {
            this.master = master;
            this.getPlayback = getPlayback;
        }

        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (args is IMouseEventArgs mouseArgs)
                return TryHandleMouseEvent(mouseArgs);
            return false;
        }

        private bool TryHandleMouseEvent(IMouseEventArgs args)
        {
            if (!args.IsLeftClickEvent() && !args.IsRightClickEvent())
                return false;
            var playback = getPlayback(master);
            if (playback == null)
                return false;

            if (!TryGetPoint(args, out var point))
                return false;
            var videoStartFactor = point.X; // point.X is in the domain [0,1]
            var timestamp = videoStartFactor * playback.Movie.Duration;

            switch (playback.State)
            {
                case MoviePlaybackState.Stopped:
                case MoviePlaybackState.Paused:
                    if (args.IsLeftClickEvent())
                    {
                        playback.Play();
                    }
                    else
                    {
                        playback.SeekToTimestamp(timestamp);
                    }
                    return true;
                case MoviePlaybackState.Playing:
                    if (args.IsLeftClickEvent())
                    {
                        playback.Pause();
                    }
                    else
                    {
                        playback.Pause();
                        playback.SeekToTimestamp(timestamp);
                        playback.Play();
                    }
                    return true;
                case MoviePlaybackState.End:
                    if (args.IsLeftClickEvent())
                    {
                        playback.SeekToTimestamp(0);
                        playback.Play();
                    }
                    else
                    {
                        playback.SeekToTimestamp(timestamp);
                        playback.Pause();
                    }
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool TryGetPoint(IMouseEventArgs mouseEventArgs, out Vector2 point)
        {
            // todo: calculate this in rayHitService
            point = default(Vector2);
            var globalRay = mouseEventArgs.Viewport.GetGlobalRayForPixelPos(mouseEventArgs.State.Position);
            var node = master.Node;
            var placementSurface = node.PresentationInfra().Placement;
            if (!placementSurface.PlacementSurface2D.TryFindPoint2D(globalRay, out var plainPoint))
                return false;
            var rect = node.GetComponent<IRectangleComponent>().Rectangle;
            if (plainPoint.Y <= rect.MinY) // We don't wish to use video's location for playing/pausing
                                              // We prefer interaction with buttons in this case
                return false;
            point = new Vector2(
                (plainPoint.X - rect.MinX) / rect.Width,
                (rect.MaxY - plainPoint.Y) / rect.Height);
            return true;
        }
    }
}