using Assets.Scripts.Infra;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Gui.MessagePopups;
using Clarity.Engine.Platforms;

namespace Assets.Scripts.Gui
{
    public class UcMessagePopupService : IMessagePopupService
    {
        private const float MessageShowTime = 3;

        private readonly IGlobalObjectService globalObjectService;

        private string currentMessage;
        private float remainingTime;

        public UcMessagePopupService(IGlobalObjectService globalObjectService, IEventRoutingService eventRoutingService)
        {
            this.globalObjectService = globalObjectService;
            eventRoutingService.Subscribe<INewFrameEvent>(typeof(UcMessagePopupService), nameof(OnNewFrame), OnNewFrame);
        }

        public void Show(string message)
        {
            currentMessage = message;
            remainingTime = MessageShowTime;
            globalObjectService.MessagePopupText.text = currentMessage;
        }

        private void OnNewFrame(INewFrameEvent evnt)
        {
            if (currentMessage == null)
                return;
            remainingTime -= evnt.FrameTime.DeltaSeconds;
            if (!(remainingTime <= 0))
                return;
            currentMessage = null;
            remainingTime = 0;
            globalObjectService.MessagePopupText.text = "";
        }
    }
}