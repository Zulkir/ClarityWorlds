//using System;

using System.Linq;
using Clarity.App.Worlds.Views;
using Clarity.Engine.Audio;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Platforms;

//using OpenTK.Audio.OpenAL;

namespace Clarity.Ext.Audio.Oal
{
    public class OalAudioRuntime : IOalAudioRuntime
    {
        private readonly IViewService viewService;

        public OalAudioRuntime(IViewService viewService)
        {
            this.viewService = viewService;
        }

        public void Update(FrameTime frameTime)
        {
            var renderingArea = viewService.RenderControl;
            var scene = renderingArea.Viewports.First().View.Layers.First().VisibleScene;
            var sceneRoot = scene.Root;
            if (scene == null || sceneRoot == null)
            {
                return;
            }

            foreach (var node in sceneRoot.EnumerateSceneNodesDeep())
            {
                var aAudio = node.SearchComponent<AudioComponent>();
                if (aAudio == null)
                    continue;
                // todo
                PlayAudio(aAudio);
            }
        }

        private unsafe void PlayAudio(AudioComponent aAudio)
        {
            /*
            if (aAudio.MoviePlayback == null) // No movie, don't play sound!
                return;
            var device = Alc.OpenDevice(null);

            if (device == null)
                throw new Exception("Could not open audio device");


            var context = Alc.CreateContext(device, (int*)null);
            Alc.MakeContextCurrent(context);

            // Check for EAX 2.0 support 
            var isEnumerationPresent = Alc.IsExtensionPresent(device, "ALC_ENUMERATION_EXT");
            if (!isEnumerationPresent)
                throw new Exception("Support for ALC_ENUMERATION_EXT doesn't not exist");

            AL.GetError(); // Clear error code

            var gBuffer = AL.GenBuffer(); // todo Fix here
            var alError = AL.GetError();
            if (alError != ALError.NoError)
                throw new Exception("alGenBuffer:" + AL.GetErrorString(alError));

            // pAudioCodecContext->
            // Copy audio data into AL Buffer 0 


            fixed (int* pData = SineWave)
                AL.BufferData(gBuffer, ALFormat.Stereo16, (IntPtr)pData,
                    sizeof(int) * SineWave.Length,
                    44100); // todo Fix here
            throw new Exception("77");
            AL.BufferData(gBuffer, ALFormat.Stereo16, aAudio.Buffer,
            aAudio.Buffer.Length,
            aAudio.Freq); // todo Fix here

            alError = AL.GetError();
            if (alError != ALError.NoError)
            {
                AL.DeleteBuffer(gBuffer);
                throw new Exception("alBufferData buffer :" + AL.GetErrorString(alError));
            }

            // Unload audio data
            AL.GenSources(1, out int alSource);
            alError = AL.GetError();
            if (alError != ALError.NoError)
                throw new Exception("alGenSources 1: " + AL.GetErrorString(alError));

            //  AL.Listener(ALListener3f.Position,0.0f,0.0f,0.0f);
            // Attach buffer 0 to source
            AL.BindBufferToSource(alSource, gBuffer);


            AL.SourceQueueBuffers(alSource, 1, new[] {gBuffer});
            alError = AL.GetError();
            if (alError != ALError.NoError)
                throw new Exception("alSourcei AL_BUFFER 0: " + AL.GetErrorString(alError));

            AL.SourcePlay(alSource);
            alError = AL.GetError();
            if (alError != ALError.NoError)
                throw new Exception("alSourcei AL_BUFFER 0: " + AL.GetErrorString(alError));

            //do { } while (AL.GetSourceState(alSource) == ALSourceState.Playing);


        }
        */
            //              Thread.Sleep(1000);
            //var sourceState = AL.GetSourceState(alSource);
            //while (sourceState == ALSourceState.Playing)
            //{
            //    sourceState = AL.GetSourceState(alSource);
            //}
            //
            //alError = AL.GetError();
            //if (alError != ALError.NoError)
            //    throw new Exception("Audio wasn't played" + AL.GetErrorString(alError));

            //AL.DeleteBuffer(gBuffer);
            //AL.DeleteSource(alSource);

        }
    }

}