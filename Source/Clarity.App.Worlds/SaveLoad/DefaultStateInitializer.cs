﻿using System;
using Clarity.App.Worlds.AppModes;
using Clarity.App.Worlds.Gui;
using Clarity.App.Worlds.Helpers;
using Clarity.App.Worlds.StoryGraph;
using Clarity.App.Worlds.StoryLayouts.NestedSpheres;
using Clarity.App.Worlds.Views;
using Clarity.App.Worlds.WorldTree;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.Gui;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Utilities;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.SaveLoad
{
    public class DefaultStateInitializer : IDefaultStateInitializer
    {
        private readonly IGui gui;
        private readonly IViewService viewService;
        private readonly IWorldTreeService worldTreeService;
        private readonly Lazy<IAppModeService> appModeServiceLazy;
        private readonly ICommonNodeFactory commonNodeFactory;
        private readonly IAmDiBasedObjectFactory objectFactory;
        private readonly IStoryService storyService;

        public DefaultStateInitializer(IViewService viewService, IWorldTreeService worldTreeService, Lazy<IAppModeService> appModeServiceLazy, 
            ICommonNodeFactory commonNodeFactory, IAmDiBasedObjectFactory objectFactory, IStoryService storyService, IGui gui)
        {
            this.viewService = viewService;
            this.worldTreeService = worldTreeService;
            this.appModeServiceLazy = appModeServiceLazy;
            this.commonNodeFactory = commonNodeFactory;
            this.objectFactory = objectFactory;
            this.storyService = storyService;
            this.gui = gui;
        }

        public void InitializeAll()
        {
            var sceneRoot = commonNodeFactory.WorldRoot(true);
            var scene = Scene.Create(sceneRoot);
            scene.BackgroundColor = Color4.White;
            scene.Root = sceneRoot;
            worldTreeService.World.Scenes.Clear();
            worldTreeService.World.Scenes.Add(scene);

            //var abstractRootComponent = world.Components.ThatAre<IStoryComponent>().Single();
            //abstractRootComponent.StartLayoutType = typeof(SphereStoryLayout);

            sceneRoot.GetComponent<IStoryComponent>().StartLayoutType = typeof(NestedSpheresStoryLayout);

            var subworld = commonNodeFactory.StoryNode();
            subworld.ChildNodes.Add(commonNodeFactory.StoryNode());
            subworld.ChildNodes.Add(commonNodeFactory.StoryNode());
            subworld.ChildNodes.Add(commonNodeFactory.StoryNode());

            sceneRoot.ChildNodes.Add(subworld);

            storyService.AddEdge(subworld.ChildNodes[0].Id, subworld.ChildNodes[1].Id);
            storyService.AddEdge(subworld.ChildNodes[1].Id, subworld.ChildNodes[2].Id);

            var renderingControl = gui.RenderControl;
            ResetEditingViewports(renderingControl, sceneRoot.GetComponent<IFocusNodeComponent>());
        }

        public void ResetEditingViewports(IRenderGuiControl renderControl, IFocusNodeComponent aMainFocusNode)
        {
            var editingView = AmFactory.Create<EditingView>();
            editingView.FocusOn(aMainFocusNode);
            var storyGraphView = AmFactory.Create<StoryGraphView>();

            var editViewport = AmFactory.Create<Viewport>();
            editViewport.View = editingView;
            var storyGraphViewport = AmFactory.Create<Viewport>();
            storyGraphViewport.View = storyGraphView;
            renderControl.SetViewports(
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

            viewService.ChangeRenderingArea(renderControl, editingView);
        }
    }
}