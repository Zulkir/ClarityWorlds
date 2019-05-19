using Clarity.Engine.Platforms;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype
{
    public class PlaybackGui
    {
        public Control Layout { get; }

        private readonly IPlayback playback;

        private readonly Button cPlay;
        private readonly Button cPlayBackwards;
        private readonly Button cIncreaseSpeed;
        private readonly Button cDecreaseSpeed;
        private readonly Button cToBeginning;
        private readonly Button cToEnd;
        private readonly Slider cSlider;

        private const float UpdateCooldown = 0.05f;
        private float remainingUpdateCooldown;

        public PlaybackGui(IPlayback playback)
        {
            this.playback = playback;
            cPlay = new Button{Text = ">"};
            cPlay.Click += (s, a) =>
            {
                if (playback.State == PlaybackState.Playing && !playback.Backwards)
                {
                    playback.State = PlaybackState.Paused;
                }
                else
                {
                    playback.State = PlaybackState.Playing;
                    playback.Backwards = false;
                }
            };

            cPlayBackwards = new Button { Text = "<" };
            cPlayBackwards.Click += (s, a) =>
            {
                if (playback.State == PlaybackState.Playing && playback.Backwards)
                {
                    playback.State = PlaybackState.Paused;
                }
                else
                {
                    playback.State = PlaybackState.Playing;
                    playback.Backwards = true;
                }
            };

            cIncreaseSpeed = new Button{Text = ">>"};
            cIncreaseSpeed.Click += (s, a) =>
            {
                if (!playback.Backwards)
                    playback.Speed *= 2;
                else
                    playback.Speed /= 2;
            };

            cDecreaseSpeed = new Button { Text = "<<" };
            cDecreaseSpeed.Click += (s, a) =>
            {
                if (!playback.Backwards)
                    playback.Speed /= 2;
                else
                    playback.Speed *= 2;
            };

            cToBeginning = new Button{Text = "|<"};
            cToBeginning.Click += (s, a) =>
            {
                playback.State = PlaybackState.Paused;
                playback.SeekRelative(0);
            };

            cToEnd = new Button { Text = ">|" };
            cToEnd.Click += (s, a) =>
            {
                playback.State = PlaybackState.Paused;
                playback.SeekRelative(1);
            };

            cSlider = new Slider
            {
                MinValue = 0,
                MaxValue = 1000
            };

            Layout = new TableLayout(
                new TableRow(
                    cToBeginning, cDecreaseSpeed, cPlayBackwards, cPlay, cIncreaseSpeed, cToEnd, cSlider
                    ));
        }

        public void Update(FrameTime frameTime)
        {
            remainingUpdateCooldown -= frameTime.DeltaSeconds;
            if (remainingUpdateCooldown > 0)
                return;
            Actualize();
            remainingUpdateCooldown = UpdateCooldown;
        }

        private void Actualize()
        {
            cSlider.Value = (int)(playback.RelativeTime * 1000);
        }
    }
}