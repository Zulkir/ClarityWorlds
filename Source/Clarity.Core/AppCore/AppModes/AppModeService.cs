using System;
using Clarity.Core.AppCore.Gui;
using Clarity.Core.AppCore.SaveLoad;
using Clarity.Core.AppCore.Views;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Core.AppCore.AppModes
{
    public class AppModeService : IAppModeService
    {
        public AppMode Mode { get; private set; }
        public event Action ModeChanged;

        private readonly IWorldTreeService worldTreeService;
        private readonly Lazy<IViewService> viewServiceLazy;
        private readonly IPresentationWorldBuilder presentationWorldBuilder;
        private readonly Lazy<IDefaultStateInitializer> defaultStateInitializerLazy;
        private readonly Lazy<IGui> guiLazy;
        private readonly Lazy<INavigationService> navigationServiceLazy;

        private IViewService ViewService => viewServiceLazy.Value;
        private IDefaultStateInitializer DefaultStateInitializer => defaultStateInitializerLazy.Value;
        private INavigationService NavigationService => navigationServiceLazy.Value;
        private IGui Gui => guiLazy.Value;

        private IWorld editingWorld;

        public AppModeService(IWorldTreeService worldTreeService, IPresentationWorldBuilder presentationWorldBuilder, Lazy<IViewService> viewServiceLazy, Lazy<IDefaultStateInitializer> defaultStateInitializerLazy, Lazy<IGui> guiLazy, Lazy<INavigationService> navigationServiceLazy)
        {
            this.worldTreeService = worldTreeService;
            this.presentationWorldBuilder = presentationWorldBuilder;
            this.viewServiceLazy = viewServiceLazy;
            this.defaultStateInitializerLazy = defaultStateInitializerLazy;
            this.guiLazy = guiLazy;
            this.navigationServiceLazy = navigationServiceLazy;
        }

        public void SetMode(AppMode mode)
        {
            //if (mode == Mode)
            //    return;
            Mode = mode;
            switch (mode)
            {
                case AppMode.Editing:
                    InitiateEditingMode();
                    break;
                case AppMode.Presentation:
                    InitiatePresentationMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
            ModeChanged?.Invoke();
        }

        // todo: change find-by-name to "OriginalNode" property (or Id)

        private void InitiatePresentationMode()
        {
            ViewService.SelectedNode = null;
            editingWorld = worldTreeService.World;
            var presentationWorld = presentationWorldBuilder.BuildPreesentationWorld(editingWorld);
            
            var focusNodeId = ViewService.MainView.FocusNode.Id;
            worldTreeService.World = presentationWorld;
            worldTreeService.ParentWorld = editingWorld;

            // todo: to DefaultStateInitializer
            var focusedPresentationNode = worldTreeService.GetById(focusNodeId);
            
            var view = AmFactory.Create<PresentationView>();
            view.FocusOn(focusedPresentationNode.GetComponent<IFocusNodeComponent>());
            
            var viewport = AmFactory.Create<Viewport>();
            viewport.View = view;

            Gui.SwitchToPresentationMode();
            var renderArea = Gui.RenderControl;
            renderArea.SetViewports(
                new [] {viewport}, 
                new ViewportsLayout
                {
                    RowHeights = new[] {new ViewportLength(100, ViewportLengthUnit.Percent)},
                    ColumnWidths = new[] {new ViewportLength(100, ViewportLengthUnit.Percent)},
                    ViewportIndices = new[,]{{0}}
                });

            ViewService.ChangeRenderingArea(renderArea, view);
            NavigationService.Reset(focusNodeId);
        }

        private void InitiateEditingMode()
        {
            var focusNodeId = ViewService.MainView.FocusNode.Id;
            worldTreeService.ParentWorld = null;
            worldTreeService.World = editingWorld;
            var focusedEditingNode = worldTreeService.GetById(focusNodeId);
            Gui.SwitchToEditMode();
            var renderArea = Gui.RenderControl;
            DefaultStateInitializer.ResetEditingViewports(renderArea, focusedEditingNode.GetComponent<IFocusNodeComponent>());
        }
    }
}