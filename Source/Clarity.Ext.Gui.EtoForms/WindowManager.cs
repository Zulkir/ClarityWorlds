using System;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.Ext.Gui.EtoForms
{
    public class WindowManager : IWindowManager
    {
        private readonly IMainForm mainForm;
        private readonly IRenderGuiControl[] renderControls;
        private readonly IWorldTreeService worldTreeService;

        public WindowManager(IMainForm mainForm, IEventRoutingService eventRoutingService, IWorldTreeService worldTreeService)
        {
            this.mainForm = mainForm;
            this.worldTreeService = worldTreeService;
            renderControls = new IRenderGuiControl[]{ mainForm.RenderControl };
            eventRoutingService.RegisterServiceDependency(typeof(IWindowManager), typeof(IWorldTreeService));
            eventRoutingService.Subscribe<IAppModeChangedEvent>(typeof(IWindowManager), nameof(OnAppModeChanged), OnAppModeChanged);
            eventRoutingService.SubscribeToAllAfter(typeof(IWindowManager).FullName, OnEveryEvent, true);
        }

        private void OnAppModeChanged(IAppModeChangedEvent evnt)
        {
            switch (evnt.NewAppMode)
            {
                case AppMode.Editing:
                {
                    var presentationView = (IFocusableView)renderControls[0].Viewports[0].View;
                    var focusNodeId = presentationView.FocusNode.Id;
                    var focusedEditingNode = worldTreeService.EditingWorld.GetNodeById(focusNodeId);
                    
                    var editingView = AmFactory.Create<EditingView>();
                    editingView.FocusOn(focusedEditingNode.GetComponent<IFocusNodeComponent>());
                    var storyGraphView = AmFactory.Create<StoryGraphView>();
                    var editViewport = AmFactory.Create<Viewport>();
                    editViewport.View = editingView;
                    var storyGraphViewport = AmFactory.Create<Viewport>();
                    storyGraphViewport.View = storyGraphView;
                    mainForm.RenderControl.SetViewports(
                        new [] {editViewport, storyGraphViewport}, 
                        new ViewportsLayout
                        {
                            RowHeights = new[]
                            {
                                new ViewportLength(70, ViewportLengthUnit.Percent),
                                new ViewportLength(30, ViewportLengthUnit.Percent),
                            },
                            ColumnWidths = new[]
                            {
                                new ViewportLength(100, ViewportLengthUnit.Percent)
                            },
                            ViewportIndices = new[,] { { 0 }, { 1 } }
                        });
                    mainForm.RenderControl.EndFullscreen();
                    break;
                }
                case AppMode.Presentation:
                {
                    var editingView = (IFocusableView)renderControls[0].Viewports[0].View;
                    var focusNodeId = editingView.FocusNode.Id;
                    var focusedPresentationNode = worldTreeService.PresentationWorld.GetNodeById(focusNodeId);
                    
                    var presentationView = AmFactory.Create<PresentationView>();
                    presentationView.FocusOn(focusedPresentationNode.GetComponent<IFocusNodeComponent>());
                    var viewport = AmFactory.Create<Viewport>();
                    viewport.View = presentationView;
                    mainForm.RenderControl.GoFullscreen();
                    mainForm.RenderControl.SetViewports(
                        new [] {viewport}, 
                        new ViewportsLayout
                        {
                            RowHeights = new[] {new ViewportLength(100, ViewportLengthUnit.Percent)},
                            ColumnWidths = new[] {new ViewportLength(100, ViewportLengthUnit.Percent)},
                            ViewportIndices = new[,]{{0}}
                        });
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEveryEvent(IRoutedEvent evnt)
        {
            foreach (var renderControl in renderControls)
            foreach (var viewport in renderControl.Viewports)
                viewport.View.OnEveryEvent(evnt);
        }
    }
}