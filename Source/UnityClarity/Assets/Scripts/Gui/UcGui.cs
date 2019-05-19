using System.Linq;
using Assets.Scripts.Rendering;
using Clarity.Common.Infra.Di;
using Clarity.Core.AppCore.AppModes;
using Clarity.Core.AppCore.Gui;
using Clarity.Core.AppCore.SaveLoad;
using Clarity.Engine.Gui;
using Clarity.Engine.Platforms;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class UcGui : IGui
    {
        private readonly IDiContainer di;
        private readonly IEventSender eventSender;

        public IRenderGuiControl RenderControl { get; }

        public UcGui(IDiContainer di, IEventSender eventSender)
        {
            this.di = di;
            this.eventSender = eventSender;
            RenderControl = new UcRenderGuiControl();
            //viewService.ChangeRenderingArea(RenderControl);
        }

        public void SwitchToPresentationMode()
        {
        }

        public void SwitchToEditMode()
        {
        }

        public void Run()
        {
            //var assetService = di.Get<IAssetService>();
            //var assetLoaders = di.GetMulti<IAssetLoader>();
            //var imageLoader = assetLoaders.ThatAre<UcImageAssetLoader>().First();

            SetupLoop();

            var saveLoadService = di.Get<ISaveLoadService>();
            saveLoadService.Format = di.GetMulti<ISaveLoadFormat>().First(x => x is ZipSaveLoadFormat);
            saveLoadService.FileName = "C:/clarity/UnityTestWorld.cw";
            saveLoadService.Load();
            var appModeService = di.Get<IAppModeService>();
            appModeService.SetMode(AppMode.Presentation);
        }

        private void SetupLoop()
        {
            var renderLoopDispatcher = di.Get<IRenderLoopDispatcher>();
            var inputProviders = di.GetMulti<IUcInputProvider>();
            eventSender.OnGUIEvent += () =>
            {
                foreach (var inputProvider in inputProviders)
                    inputProvider.OnGui();
            };
            eventSender.UpdateEvent += () =>
            {
                foreach (var inputProvider in inputProviders)
                    inputProvider.OnUpdate();
                renderLoopDispatcher.OnLoop(new FrameTime(Time.realtimeSinceStartup, Time.deltaTime));
            };
        }
    }
}