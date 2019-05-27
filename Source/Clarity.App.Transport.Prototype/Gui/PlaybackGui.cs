using System.Globalization;
using Clarity.App.Transport.Prototype.Runtime;
using Clarity.Engine.Platforms;
using Eto.Forms;

namespace Clarity.App.Transport.Prototype.Gui
{
    public class PlaybackGui
    {
        public Control Layout { get; }

        private readonly IPlaybackService playbackService;

        private readonly Button cPlay;
        private readonly Button cPlayBackwards;
        private readonly Button cIncreaseSpeed;
        private readonly Button cDecreaseSpeed;
        private readonly Button cToBeginning;
        private readonly Button cToEnd;
        private readonly Slider cSlider;
        private readonly Label cTime;

        private const float UpdateCooldown = 0.05f;
        private float remainingUpdateCooldown;

        private bool isActualizing;

        public PlaybackGui(IPlaybackService playbackService)
        {
            this.playbackService = playbackService;
            cPlay = new Button{Text = ">"};
            cPlay.Click += (s, a) =>
            {
                if (playbackService.State == PlaybackState.Playing && !playbackService.Backwards)
                {
                    playbackService.State = PlaybackState.Paused;
                }
                else
                {
                    playbackService.State = PlaybackState.Playing;
                    playbackService.Backwards = false;
                }
                Actualize();
            };

            cPlayBackwards = new Button { Text = "<" };
            cPlayBackwards.Click += (s, a) =>
            {
                if (playbackService.State == PlaybackState.Playing && playbackService.Backwards)
                {
                    playbackService.State = PlaybackState.Paused;
                }
                else
                {
                    playbackService.State = PlaybackState.Playing;
                    playbackService.Backwards = true;
                }
                Actualize();
            };

            cIncreaseSpeed = new Button{Text = ">>"};
            cIncreaseSpeed.Click += (s, a) =>
            {
                if (!playbackService.Backwards)
                    playbackService.Speed *= 2;
                else
                    playbackService.Speed /= 2;
            };

            cDecreaseSpeed = new Button { Text = "<<" };
            cDecreaseSpeed.Click += (s, a) =>
            {
                if (!playbackService.Backwards)
                    playbackService.Speed /= 2;
                else
                    playbackService.Speed *= 2;
            };

            cToBeginning = new Button{Text = "|<"};
            cToBeginning.Click += (s, a) =>
            {
                playbackService.State = PlaybackState.Paused;
                playbackService.RelativeTime = 0;
            };

            cToEnd = new Button { Text = ">|" };
            cToEnd.Click += (s, a) =>
            {
                playbackService.State = PlaybackState.Paused;
                playbackService.RelativeTime = 1;
            };

            cSlider = new Slider
            {
                MinValue = 0,
                MaxValue = 1000,
            };
            cSlider.ValueChanged += (s, a) =>
            {
                if (isActualizing)
                    return;
                playbackService.RelativeTime = cSlider.Value / 1000f;
                Actualize();
            };

            cTime = new Label
            {
                Width = 100,
            };

            Layout = new TableLayout(
                new TableRow(
                    cToBeginning, cDecreaseSpeed, cPlayBackwards, cPlay, cIncreaseSpeed, cToEnd, 
                    cTime, cSlider
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
            isActualizing = true;
            cSlider.Value = (int)(playbackService.RelativeTime * 1000);
            cTime.Text = playbackService.AbsoluteTime.ToString("N", CultureInfo.InvariantCulture);
            cPlay.Text = playbackService.State == PlaybackState.Playing && !playbackService.Backwards ? "| |" : ">";
            cPlayBackwards.Text = playbackService.State == PlaybackState.Playing && playbackService.Backwards ? "| |" : "<";
            isActualizing = false;
        }
    }
}