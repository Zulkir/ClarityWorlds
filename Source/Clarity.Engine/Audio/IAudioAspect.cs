using Clarity.Engine.Media.Movies;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Audio
{
    public interface IAudioAspect
    {
        IMoviePlayback MoviePlayback { get; }
 //       public abstract byte[] Buffer { get; }
 //       public abstract int Freq { get; }
        double StartTimestamp { get; }
        ISceneNode Node { get; }
    }

    public class AudioAspect : IAudioAspect
    {

        public AudioAspect(ISceneNode node)
        {
            Node = node;
  //          MoviePlayback = Node.GetAspect<IRectangleComponent>().MoviePlayback;
            /*MoviePlayback =*/ // Node.GetAspect<IMovie>();
            // Grab playback from node
        }

        public IMoviePlayback MoviePlayback { get; }
//        public override byte[] Buffer => MoviePlayback?.GetAudioBuffer();
//        public override int Freq => MoviePlayback.GetAudioFreq();
        public double StartTimestamp => MoviePlayback.FrameTimestamp;
        public ISceneNode Node { get; }
    }
}