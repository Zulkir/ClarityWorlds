using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Core.AppCore.AppModes;
using Clarity.Core.AppCore.Interaction;
using Clarity.Core.AppCore.Views;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Interaction.RayHittables.Embedded;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Movies;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Components;
using Clarity.Engine.Visualization.Graphics;
using Clarity.Engine.Visualization.Graphics.Materials;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Core.AppCore.WorldTree
{
    public abstract class MovieRectangleComponent : SceneNodeComponentBase<MovieRectangleComponent>,
        IVisualComponent, IInteractionComponent, IRayHittableComponent
    {
        private enum MovieButton
        {
            Start,
            FBwrd,
            Bwrd,
            Play,
            FFrwd,
            End
        };

        private enum ButtonTextureName
        {
            Start,
            FBwrd,
            Bwrd,
            Pause,
            Frwd,
            FFrwd,
            End
        };

        private const int BarHeightFactor = 32;
        private const float ButtonHalfWidth = 0.05f;
        private const float ButtonHalfHeight = 0.05f;
        private const float ButtonHorizontalMargin = ButtonHalfWidth;
        private const float MovieGuiTimeout = 2f;

        private static readonly MovieButton[] AllMovieButtons = (MovieButton[])Enum.GetValues(typeof(MovieButton));

        private static readonly ButtonTextureName[] AllButtonTextureNames =
            (ButtonTextureName[])Enum.GetValues(typeof(ButtonTextureName));

        public abstract IMovie Movie { get; set; }

        private readonly Lazy<IMoviePlayer> moviePlayerLazy;
        private readonly Lazy<IAppModeService> appModeServiceLazy;
        
        private readonly List<IVisualElement> visualElems;
        private readonly IVisualElement visualElem;
        private readonly IRayHittable hittable;

        private readonly IPixelSource fallbackPixelSource;
        private readonly IPixelSource barBox;

        private readonly IVisualElement progressBar;
        private readonly IImage[] movieSpeedTextures;
        private readonly IInteractionElement[] presentationInteractionElems;
        private readonly IStandardMaterial[] movieButtonsMaterials;
        private readonly IEnumerable<IVisualElement> movieButtonVisualElems;
        private readonly IVisualElement movieSpeedVisualElem;

        private IMoviePlayback moviePlayback;
        private float lastUpdateTime;
        private bool showMovieGui;
        private float lastMouseEventTime;

        private IMoviePlayer MoviePlayer => moviePlayerLazy.Value;
        private AaRectangle2 Rectangle => Node.SearchComponent<IRectangleComponent>()?.Rectangle ?? new AaRectangle2();

        protected MovieRectangleComponent(Lazy<IMoviePlayer> moviePlayerLazy, Lazy<IAppModeService> appModeServiceLazy,
            IEmbeddedResources embeddedResources, IViewService viewService)
        {
            this.moviePlayerLazy = moviePlayerLazy;
            this.appModeServiceLazy = appModeServiceLazy;

            var model = embeddedResources.SimplePlaneXyModel();
            var mainMaterial = new StandardMaterialProxy<MovieRectangleComponent>(this)
            {
                GetDiffuseTextureSource = x => x.moviePlayback ?? x.fallbackPixelSource,
                GetIgnoreLighting = x => true
            };
            visualElem = new CgModelVisualElement<MovieRectangleComponent>(this)
                .SetModel(model)
                .SetMaterial(mainMaterial)
                .SetTransform(x => Transform.Translation(new Vector3(x.Rectangle.Center, 0)))
                .SetNonUniformScale(x => new Vector3(x.Rectangle.HalfWidth, x.Rectangle.HalfHeight, 1));

            fallbackPixelSource = new SingleColorPixelSource(Color4.Black);

            barBox = new SingleColorPixelSourceProxy<MovieRectangleComponent>(this) {GetColor = x => Color4.Red};

            IStandardMaterial barMaterial = new StandardMaterialProxy<MovieRectangleComponent>(this)
            {
                GetDiffuseTextureSource = x => x.barBox,
                GetIgnoreLighting = x => true
            };

            progressBar = new CgModelVisualElement<MovieRectangleComponent>(this)
                .SetModel(model)
                .SetMaterial(barMaterial)
                .SetTransform(x => Transform.Translation(new Vector3(
                    x.Rectangle.MinX + x.Rectangle.HalfWidth * MovieRelativeLocation(),
                    x.Rectangle.MinY + x.Rectangle.HalfHeight / BarHeightFactor, 0)))
                .SetNonUniformScale(x => new Vector3(
                    x.Rectangle.HalfWidth * MovieRelativeLocation(),
                    x.Rectangle.HalfHeight / BarHeightFactor, 1))
                .SetHide(x => !x.showMovieGui)
                .SetZOffset(GraphicsHelper.MinZOffset);

            var movieButtonTextures = AllButtonTextureNames
                .Select(x => embeddedResources.Image(GetMovieButtonTextureFileName(x.ToString())))
                .ToArray();

            movieButtonsMaterials = AllButtonTextureNames.Select(x =>
                    new StandardMaterial(movieButtonTextures[(int)x])
                    {
                        IgnoreLighting = true,
                    })
                .Cast<IStandardMaterial>()
                .ToArray();

            movieButtonVisualElems = AllMovieButtons.Select(b => new CgModelVisualElement<MovieRectangleComponent>(this)
                .SetModel(model)
                .SetMaterial(x => x.movieButtonsMaterials[(int)GetButtonTextureName(b)])
                .SetTransform(x => GetMovieButtonTransform((int)b, x.Rectangle))
                .SetHide(x => !x.showMovieGui)
                .SetZOffset(GraphicsHelper.MinZOffset));

            movieSpeedTextures = StandardMoviePlayback.MovieSpeeds.Select(x =>
                embeddedResources.Image(GetMovieSpeedTextureFileName(x))).ToArray();

            IStandardMaterial movieSpeedMaterial = new StandardMaterialProxy<MovieRectangleComponent>(this)
            {
                GetDiffuseTextureSource = x => x.movieSpeedTextures[GetSpeedIndex(moviePlayback.GetVideoSpeed())]
                //DiffuseTextureSource = new CgImageTexture2DSource(movieSpeedTexture),
                //IgnoreLighting = true,
            };
            movieSpeedVisualElem = new CgModelVisualElement<MovieRectangleComponent>(this)
                .SetModel(model)
                .SetMaterial(movieSpeedMaterial)
                .SetTransform(x => GetMovieSpeedTransform(x.Rectangle))
                .SetHide(x => !x.showMovieGui)
                .SetZOffset(GraphicsHelper.MinZOffset);

            presentationInteractionElems = new IInteractionElement[]
            {
                new MoviePlaybackInteractionElement<MovieRectangleComponent>(this, x => x.moviePlayback),
                new LambdaInteractionElement(args =>
                {
                    if (!(args is MouseEventArgs mouseArgs))
                        return true;
                    lastMouseEventTime = lastUpdateTime;

                    if (!mouseArgs.IsLeftClickEvent())
                        return true;
                    if (!TryGetButton(mouseArgs, out var button))
                        return true;
                    switch (button)
                    {
                        case MovieButton.Start:
                            moviePlayback.GoToStart();
                            break;
                        case MovieButton.FBwrd:
                            moviePlayback.PlaySlower();
                            break;
                        case MovieButton.Bwrd:
                            moviePlayback.ReverseDirection();
                            break;
                        case MovieButton.Play:
                            moviePlayback.UpdatePlayStatus();
                            break;
                        case MovieButton.FFrwd:
                            moviePlayback.PlayFaster();
                            break;
                        case MovieButton.End:
                            moviePlayback.GoToEnd();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    // Process button click
                    return true;
                })
            };

            hittable = new RectangleHittable<MovieRectangleComponent>(this, Transform.Identity,
                c => c.GetHittableRectangle(), c => 0);

            visualElems = new List<IVisualElement> {visualElem};
        }

        private float MovieRelativeLocation()
        {
            return (float)(moviePlayback.FrameTimestamp / Movie.Duration);
        }

        private int GetSpeedIndex(double speed)
        {
            for (var sIndex = 0; sIndex < StandardMoviePlayback.MovieSpeeds.Length; sIndex++)
            {
                if (StandardMoviePlayback.MovieSpeeds[sIndex] == speed)
                    return sIndex;
            }
            throw new Exception("Speed not found");
        }

        private ButtonTextureName GetButtonTextureName(MovieButton button)
        {
            switch (button)
            {
                case MovieButton.Start: return ButtonTextureName.Start;
                case MovieButton.FBwrd: return ButtonTextureName.FBwrd;
                case MovieButton.Bwrd: return ButtonTextureName.Bwrd;
                case MovieButton.Play:
                    return moviePlayback.State == MoviePlaybackState.Playing
                        ? ButtonTextureName.Pause
                        : ButtonTextureName.Frwd;
                case MovieButton.FFrwd: return ButtonTextureName.FFrwd;
                case MovieButton.End: return ButtonTextureName.End;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }

        private static string GetMovieButtonTextureFileName(string button) =>
            $"Textures/MovieButtons/{button}.png";

        private static string GetMovieSpeedTextureFileName(double speed) =>
            $"Textures/Xspeed/{speed.ToString(CultureInfo.InvariantCulture)}x.png";

        private static Transform GetMovieButtonTransform(int index, AaRectangle2 rect)
        {
            return new Transform(ButtonHalfHeight, Quaternion.Identity, new Vector3(
                rect.MinX + ButtonHalfHeight +
                ((int)index) * ((rect.Width - 2 * ButtonHalfWidth) / (AllMovieButtons.Length - 1)),
                rect.MinY - ButtonHalfHeight, 0));
        }


        private static Transform GetMovieSpeedTransform(AaRectangle2 rect)
        {
            return new Transform(ButtonHalfHeight, Quaternion.Identity, new Vector3(
                rect.MinX + ButtonHalfHeight,
                rect.MaxY - ButtonHalfHeight, 0));
        }

        private AaRectangle2 GetHittableRectangle()
        {
            if (Movie == null)
                return Rectangle;
            var buttonPanelRect = CalculateButtonPanelRectangle();
            var minX = Math.Min(Rectangle.MinX, buttonPanelRect.MinX);
            var minY = Math.Min(Rectangle.MinY, buttonPanelRect.MinY);
            var halfWidth = Math.Max(Rectangle.Width, buttonPanelRect.Width) / 2;
            var halfHeight = (Rectangle.Height + buttonPanelRect.Height) / 2;
            var center = new Vector2(minX, minY) + new Vector2(halfWidth, halfHeight);
            return new AaRectangle2(center, halfWidth, halfHeight);
        }

        private AaRectangle2 CalculateButtonPanelRectangle()
        {
            var buttonPanelWidth = AllMovieButtons.Length * 2 * ButtonHalfWidth +
                                   (AllMovieButtons.Length - 1) * ButtonHorizontalMargin;
            var buttonPanelLeftX = Rectangle.Center.X - buttonPanelWidth / 2;
            var buttonPanelBottomY = Rectangle.MinY - (ButtonHalfHeight * 2);
            var buttonPanelRect =
                AaRectangle2.FromCornerAndDimensions(buttonPanelLeftX, buttonPanelBottomY, buttonPanelWidth, ButtonHalfHeight * 2);
            return buttonPanelRect;
        }

        public override void Update(FrameTime frameTime)
        {
            if (Movie == null)
                return;
            if (moviePlayback == null || moviePlayback.Movie != Movie)
            {
                moviePlayback?.Dispose();
                moviePlayback = MoviePlayer.CreatePlayback(Movie);
            }
            lastUpdateTime = frameTime.TotalSeconds;
            showMovieGui = frameTime.TotalSeconds - lastMouseEventTime < MovieGuiTimeout;
            moviePlayback.OnUpdate(frameTime);
//            movieAudioRuntime.OnUpdate(frameTime);
        }

        public override void AmOnChildEvent(IAmEventMessage message)
        {
            if (message.Obj(this).ValueChanged(x => x.Movie, out _))
                FillVisualElems();
            base.AmOnChildEvent(message);
        }

        private void FillVisualElems()
        {
            visualElems.Clear();
            visualElems.Add(visualElem);
            if (Movie != null)
            {
                visualElems.AddRange(movieButtonVisualElems);
                visualElems.Add(progressBar);
                visualElems.Add(movieSpeedVisualElem);
            }
        }

        // todo: remove from here
        private bool TryGetButton(IMouseEventArgs mouseEventArgs, out MovieButton button)
        {
            button = MovieButton.Start; // Default
            var globalRay = mouseEventArgs.Viewport.GetGlobalRayForPixelPos(mouseEventArgs.State.Position);
            var aPlacement = Node.PresentationInfra().PlacementNodeAspect;
            var placementPlane = aPlacement.PlacementPlane;
            if (!placementPlane.TryFindPoint2D(globalRay, out var plainPoint))
                return false;
            var rect = Rectangle;
            if (plainPoint.Y > rect.MinY)
                return false;
            if (((plainPoint.X - rect.MinX) %
                 ((rect.Width - 2 * ButtonHalfWidth) / (AllMovieButtons.Length - 1))) >=
                2 * ButtonHalfWidth)
                return false;
            button = (MovieButton)(int)((plainPoint.X - rect.MinX) /
                                        ((rect.Width - 2 * ButtonHalfWidth) / (AllMovieButtons.Length - 1)));
            return true;
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements() => visualElems;

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEventArgs args)
        {
            if (appModeServiceLazy.Value.Mode == AppMode.Presentation)
                foreach (var element in presentationInteractionElems)
                    if (element.TryHandleInteractionEvent(args))
                        return true;
            return false;
        }

        // Hittable
        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable.HitWithClick(clickInfo);
    }
}