using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Audio;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Audio.Oal
{
    public class OalAudioSystem : IAudioSystem
    {
        public OalAudioSystem(IDiContainer di, IRenderLoopDispatcher renderLoopDispatcher)
        {
            di.Bind<IOalAudioRuntime>().AsLastChoice.To<OalAudioRuntime>();

            var runtime = di.Get<IOalAudioRuntime>();
            renderLoopDispatcher.Update += runtime.Update;
        }
    }
}