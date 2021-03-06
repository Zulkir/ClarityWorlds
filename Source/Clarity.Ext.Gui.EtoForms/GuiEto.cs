﻿using Clarity.App.Worlds.Gui;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui;
using Clarity.Engine.Platforms;
using Eto;

namespace Clarity.Ext.Gui.EtoForms
{
    public class GuiEto : IGui
    {
        private readonly IEventRoutingService eventRoutingService;
        private readonly Eto.Forms.Application etoApplication;
        private readonly IMainForm mainForm;
        private readonly IRenderLoopDispatcher renderLoopDispatcher;
        private readonly IFrameTimeMeasurer frameTimeMeasurer;

        public IRenderGuiControl RenderControl => mainForm.RenderControl;
        public void SwitchToPresentationMode() { mainForm.RenderControl.GoFullscreen(); }
        public void SwitchToEditMode() { mainForm.RenderControl.EndFullscreen(); }

        public GuiEto(IDiContainer di)
        {
            eventRoutingService = di.Get<IEventRoutingService>();
            var platform = di.Get<Platform>();
            platform.Add(di.Get<Eto.Forms.Application.IHandler>);
            platform.Add(di.Get<RenderControl.IHandler>);
            etoApplication = new Eto.Forms.Application(platform);
            var handler = (ILoopAppHandler)etoApplication.Handler;
            renderLoopDispatcher = di.Get<IRenderLoopDispatcher>();
            frameTimeMeasurer = di.Get<IFrameTimeMeasurer>();
            mainForm = di.Get<IMainForm>();
            handler.NewFrame += NewFrame;
        }

        private void NewFrame()
        {
            var frameTime = frameTimeMeasurer.MeasureTime();
            eventRoutingService.FireEvent<INewFrameEvent>(new NewFrameEvent(frameTime));
            // todo: remove
            renderLoopDispatcher.OnLoop(frameTime);
        }

        public void Run()
        {
            etoApplication.Run(mainForm.Form);
            renderLoopDispatcher.OnClosing();
        }
    }
}