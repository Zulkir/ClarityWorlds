using System;
using Clarity.App.Transport.Prototype.Dummy;
using Clarity.App.Transport.Prototype.FirstProto;
using Clarity.App.Transport.Prototype.FirstProto.Simulation;
using Clarity.App.Transport.Prototype.FirstProto.Visualization;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.App.Transport.Prototype.Visualization;
using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Images;
using Clarity.Engine.Media.Text.Rich;
using Clarity.Engine.Platforms;
using Clarity.Ext.Gui.EtoForms;
using Clarity.Ext.Gui.EtoForms.AppModes;
using Clarity.Ext.Gui.EtoForms.StoryGraph;
using Clarity.Ext.Gui.EtoForms.Text;
using Eto;

namespace Clarity.App.Transport.Prototype
{
    public class AppLifecycle : EngineLifecycle
    {
        public void StartAndRun(IEnvironment environment)
        {
            var di = new DiContainer();
            BindDefaults(di);
            BindExtensions(di, environment);
            InitializeStatics(di);
            StartupExtensions(di, environment);
            StartupCore(di);
            Run(di);
        }

        protected override void BindDefaults(IDiContainer di)
        {
            base.BindDefaults(di);

            di.Bind<IWindowingSystem>().To<TransportGui>();
            di.Bind<TransportGui>().To<TransportGui>();
            if (EtoEnvironment.Platform.IsWindows)
            {
                di.Bind<Platform>().To<Eto.WinForms.Platform>();
                di.Bind<Eto.Forms.Application.IHandler>().To<LoopAppHandlerWinForms>();
                di.Bind<RenderControl.IHandler>().To<RenderingAreaHandlerWinFormsOgl>();
            }
            else
            {
                throw new NotSupportedException("This platform is not supported.");
            }

            di.Bind<RenderControl>().To<RenderControl>();
            di.Bind<IRtImageBuilder>().To<RtImageBuilder>();
            di.Bind<IFontFamilyCache>().To<FontFamilyCache>();
            di.Bind<IRichTextMeasurer>().To<RichTextMeasurer>();
            di.Bind<IAppModesCommands>().To<AppModesCommands>();
            di.Bind<IStoryGraphGui>().To<StoryGraphGui>();
            di.Bind<IFrameTimeMeasurer>().To<FrameTimeMeasurer>();
            di.Bind<ICommonGuiObjects>().To<CommonGuiObjects>();
            di.Bind<IMainForm>().To<PrototypeMainForm>();
            di.Bind<IMouseInputProvider>().To<MouseInputProvider>();
            di.Bind<IKeyboardInputProvider>().To<KeyboardInputProvider>();
            di.Bind<IOldPlayback>().To<OldPlayback>();
            di.Bind<IInputHandler>().To<InputHandler>();
            di.Bind<IStateVisualizer>().To<StateVisualizer>();
            di.Bind<ISimFrameGenerator>().To<SimFrameGenerator>();
            di.Bind<ICommonMaterials>().To<CommonMaterials>();
            di.Bind<IImageLoader>().To<DummyImageLoader>();

            di.Bind<IAppRuntime>().To<AppRuntime>();
            di.Bind<ITableService>().To<TableService>();
            di.Bind<IDataSourceRuntime>().To<DataSourceRuntime>();
            di.Bind<IOriginDataLogService>().To<OriginDataLogService>();
            di.Bind<IDataQueriesService>().To<DataQueriesService>();
            di.Bind<IVisualQueriesRuntime>().To<VisualQueriesRuntime>();
            di.Bind<IPlaybackService>().To<PlaybackService>();
            di.Bind<IAppProcedures>().To<AppProcedures>();
            di.Bind<IDataRetrievalService>().To<DataRetrievalService>();
        }

        protected virtual void Run(IDiContainer di)
        {
            di.Get<IMouseInputProvider>();
            di.Get<IKeyboardInputProvider>();
            di.Get<TransportGui>().Run();
        }
    }
}