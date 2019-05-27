using Clarity.Common.Infra.DependencyInjection;
using Clarity.Engine.Audio;
using Clarity.Engine.Platforms;

namespace Clarity.Ext.Audio.Oal
{
    public class OalExtension : IExtension
    {
        public string Name => "Clarity.Ext.Audio.Oal";

        public void Bind(IDiContainer di)
        {
            di.Bind<IAudioSystem>().To<OalAudioSystem>();
        }

        public void OnStartup(IDiContainer di)
        {
        }
    }
}