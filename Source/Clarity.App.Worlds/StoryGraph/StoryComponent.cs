using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.Interaction;
using Clarity.App.Worlds.Interaction.Placement;
using Clarity.App.Worlds.Interaction.Tools;
using Clarity.App.Worlds.Navigation;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.Views.Cameras;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Common.Numericals.Geometry;
using Clarity.Common.Numericals.OtherTuples;
using Clarity.Engine.Gui;
using Clarity.Engine.Gui.Menus;
using Clarity.Engine.Interaction;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.RayHittables;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Elements;
using Clarity.Engine.Visualization.Elements.Effects;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.StoryGraph
{
    public abstract class StoryComponent : SceneNodeComponentBase<StoryComponent>, IStoryComponent, 
        IFocusNodeComponent, IVisualComponent, IInteractionComponent, IGuiComponent, IRayHittableComponent, IPlacementComponent
    {
        private static readonly IntSize2 ThumbnailSize = new IntSize2(256, 256);

        public abstract Type StartLayoutType { get; set; }
        public abstract Color4? BackgroundColor { get; set; }
        public abstract bool InstantTransition { get; set; }
        public abstract bool SkipOrder { get; set; }
        public abstract bool HideMain { get; set; }
        public abstract bool ShowAux1 { get; set; }
        public abstract bool ShowAux2 { get; set; }
        public abstract bool ShowAux3 { get; set; }
        public abstract bool ShowAux4 { get; set; }
        public abstract bool ShowAux5 { get; set; }

        public IDefaultViewpointMechanism DefaultViewpointMechanism { get; set; }
        private readonly Lazy<IAppModeService> appModeServiceLazy;
        private readonly IViewService viewService;
        private readonly ICommonNodeFactory commonNodeFactory;
        private readonly IToolService toolService;
        private readonly IToolFactory toolFactory;
        private readonly IRenderService renderService;
        private readonly Lazy<IStoryService> storyGraphServiceLazy;
        private readonly INavigationService navigationService;

        private readonly IInteractionElement selectInteractionElement;
        private readonly IInteractionElement focusInteractionElement;
        private readonly IInteractionElement navigateInteractionElement;

        private IEnumerable<IVisualElement> visualElems;
        private IEnumerable<IVisualEffect> visualEffects;
        private IGuiCommand[] guiCommandBlock;
        private IRayHittable hittable;

        private IImage thumbnail;
        private bool thumbnailDirty;

        private Action<ISceneNode, object, FrameTime> onUpdate;
        private object onUpdateClosure;

        public bool IsLayoutRoot => StartLayoutType != null;
        private IStoryService StoryService => storyGraphServiceLazy.Value;
        
        protected StoryComponent(IViewService viewService, ICommonNodeFactory commonNodeFactory, IToolService toolService, 
            IToolFactory toolFactory, IRenderService renderService, Lazy<IStoryService> storyGraphServiceLazy, 
            INavigationService navigationService, Lazy<IAppModeService> appModeServiceLazy)
        {
            this.viewService = viewService;
            this.commonNodeFactory = commonNodeFactory;
            this.toolService = toolService;
            this.toolFactory = toolFactory;
            this.renderService = renderService;
            this.storyGraphServiceLazy = storyGraphServiceLazy;
            this.navigationService = navigationService;
            this.appModeServiceLazy = appModeServiceLazy;
            ShowAux1 = true;
            ShowAux2 = true;
            selectInteractionElement = new SelectOnClickInteractionElement(this, viewService);
            focusInteractionElement = new FocusOnDoubleClickInteractionElement(this);
            navigateInteractionElement = new NavigateOnDoubleClickInteractionElement(this, navigationService);
        }

        public void SetDynamicParts(StoryNodeDynamicParts dynamicParts)
        {
            DefaultViewpointMechanism = dynamicParts.DefaultViewpointMechanism;
            visualElems = dynamicParts.VisualElements;
            visualEffects = dynamicParts.VisualEffects;
            hittable = dynamicParts.Hittable;
            onUpdate = dynamicParts.OnUpdate;
            onUpdateClosure = dynamicParts.OnUpdateClosure;
            PlacementSurface2D = dynamicParts.PlacementSurface2D ?? new DummyPlacementSurface();
            PlacementSurface3D = dynamicParts.PlacementSurface3D ?? new DummyPlacementSurface();
        }

        public override void AmOnAttached()
        {
            guiCommandBlock = new IGuiCommand[]
            {
                // todo: undo/redo
                new GuiCommand("Next into...", KeyModifyers.Control, Key.N, () =>
                {
                    toolService.CurrentTool = toolFactory.StoryBranchInto(Node.Id);
                }),
                new GuiCommand("Insert Next", KeyModifyers.Control, Key.I, () =>
                {
                    StoryService.OnBeginTransaction(this);
                    var newNode = commonNodeFactory.StoryNode();
                    var nexts = StoryService.GlobalGraph.Next[Node.Id].ToArray();
                    foreach (var next in nexts)
                        StoryService.RemoveEdge(Node.Id, next);
                    var index = Node.ParentNode.ChildNodes.IndexOf(Node);
                    Node.ParentNode.ChildNodes.Insert(index + 1, newNode);
                    StoryService.AddEdge(Node.Id, newNode.Id);
                    foreach (var nextNode in nexts)
                        StoryService.AddEdge(newNode.Id, nextNode);
                    StoryService.OnEndTransaction(this);
                }),
                new GuiCommand("Connect to...", KeyModifyers.Control, Key.C, () =>
                {
                    toolService.CurrentTool = toolFactory.AddExplicitStoryGraphEdge(Node.Id);
                }), 
                new GuiCommand("Wrap into", KeyModifyers.None, Key.None, () =>
                {
                    StoryOperations.AddChild(StoryService, commonNodeFactory, Node.Id, this);
                }),
            };
            SetDynamicParts(new StoryNodeDynamicParts
            {
                DefaultViewpointMechanism = new WallDefaultViewpointMechanism(Node),
                VisualElements = EmptyArrays<IVisualElement>.Array
            });
        }

        public override void OnNodeEvent(IAmEventMessage message)
        {
            if (message.Obj<ISceneNode>().ItemAddedOrRemoved(x => x.ChildNodes, out _))
                thumbnailDirty = true;
        }

        public IImage GetThumbnail()
        {
            if (thumbnail == null)
            {
                thumbnail = renderService.CreateRenderTargetImage(ThumbnailSize);
                renderService.Render(thumbnail, new []{Viewport.Create(IntVector2.Zero, ThumbnailSize, ThumbnailView.Create(this))}, 0);
            }
            else if (thumbnailDirty)
            {
                renderService.Render(thumbnail, new[] { Viewport.Create(IntVector2.Zero, ThumbnailSize, ThumbnailView.Create(this)) }, 0);
            }
            thumbnailDirty = false;
            return thumbnail;
        }

        public void InvalidateThumbnails()
        {
            thumbnailDirty = true;
            foreach (var aChild in EnumerateImmediateStoryChildren(false))
                aChild.Node.GetComponent<StoryComponent>().InvalidateThumbnails();
        }

        public override void Update(FrameTime frameTime)
        {
            onUpdate?.Invoke(Node, onUpdateClosure, frameTime);
        }

        public IEnumerable<IStoryComponent> EnumerateImmediateStoryChildren(bool sameLayoutOnly) =>
            sameLayoutOnly
                ? Node.ChildNodes.Select(x => x.SearchComponent<IStoryComponent>()).Where(x => x != null)
                : Node.ChildNodes.Select(x => x.SearchComponent<IStoryComponent>()).Where(x => x != null && !x.IsLayoutRoot);

        public IEnumerable<IStoryComponent> EnumerateStoryAspectsDeep(bool sameLayoutOnly)
        {
            return this.EnumSelfAs<IStoryComponent>().Concat(EnumerateImmediateStoryChildren(sameLayoutOnly).SelectMany(x => x.EnumerateStoryAspectsDeep(sameLayoutOnly)));
        }

        // Visual
        public IEnumerable<IVisualElement> GetVisualElements() => visualElems;
        public IEnumerable<IVisualEffect> GetVisualEffects() => visualEffects ?? EmptyArrays<IVisualEffect>.Array;

        // Hittable
        public RayHitResult HitWithClick(RayHitInfo clickInfo) => hittable?.HitWithClick(clickInfo) ?? RayHitResult.Failure();

        // Interaction
        public bool TryHandleInteractionEvent(IInteractionEvent args)
        {
            return appModeServiceLazy.Value.Mode == AppMode.Editing && selectInteractionElement.TryHandleInteractionEvent(args) || 
                   focusInteractionElement.TryHandleInteractionEvent(args) || 
                   navigateInteractionElement.TryHandleInteractionEvent(args);
        }

        // GUI
        public void BuildMenuSection(IGuiMenuBuilder menuBuilder)
        {
            foreach (var command in guiCommandBlock)
                menuBuilder.AddCommand(command);
        }

        // Placement
        public IPlacementSurface PlacementSurface2D { get; private set; }
        public IPlacementSurface PlacementSurface3D { get; private set; }
    }
}