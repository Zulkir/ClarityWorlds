﻿using System.Collections.Generic;
using Clarity.App.Transport.Prototype.Visualization;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Numericals;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Colors;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;
using Clarity.Engine.Visualization.Cameras;
using Clarity.Engine.Visualization.Cameras.Embedded;
using Clarity.Engine.Visualization.Viewports;
using Clarity.Engine.Visualization.Views;

namespace Clarity.App.Transport.Prototype
{
    public abstract class TransportView : AmObjectBase<TransportView, IViewport>, IView
    {
        public IReadOnlyList<IViewLayer> Layers { get; }
        private readonly IControlledCamera camera;
        private readonly ViewLayer layer;

        protected TransportView(IStateVisualizer stateVisualizer)
        {
            var scene = Scene.Create(stateVisualizer.RootNode);
            scene.BackgroundColor = Color4.CornflowerBlue;
            camera = new TargetedControlledCameraY(stateVisualizer.RootNode, new TargetedControlledCameraY.Props
            {
                Target = Vector3.Zero,
                Distance = 50f,
                Pitch = MathHelper.PiOver4,
                Yaw = MathHelper.PiOver4,
                FieldOfView = MathHelper.PiOver4,
                ZNear = 0.1f,
                ZFar = 1000f
            });
            layer = new ViewLayer
            {
                VisibleScene = scene,
                Camera = camera
            };
            Layers = new IViewLayer[]
            {
                layer,
            };
        }

        public void Update(FrameTime frameTime)
        {
            camera.Update(frameTime);
        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            return false;
        }

        public void OnEveryEvent(IRoutedEvent evnt)
        {
        }
    }
}