using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Gui;
using Clarity.Engine.Platforms;
using Clarity.Ext.Gui.EtoForms;
using Eto;

namespace Clarity.App.Transport.Prototype
{
    public class TransportGui : IWindowingSystem
    {
        private readonly Eto.Forms.Application etoApplication;
        private readonly IMainForm mainForm;
        private readonly IRenderLoopDispatcher renderLoopDispatcher;
        private readonly IFrameTimeMeasurer frameTimeMeasurer;

        public IRenderGuiControl RenderControl => mainForm.RenderControl;

        public TransportGui(IDiContainer di)
        {
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
            renderLoopDispatcher.OnLoop(frameTimeMeasurer.MeasureTime());
        }

        public void Run()
        {
            etoApplication.Run(mainForm.Form);
            renderLoopDispatcher.OnClosing();
        }
    }
}