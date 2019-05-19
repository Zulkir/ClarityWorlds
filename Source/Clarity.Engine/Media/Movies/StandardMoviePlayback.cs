using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.Numericals;
using Clarity.Engine.Platforms;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Movies
{
    public class StandardMoviePlayback : ResourceBase, IMoviePlayback
    {
        private struct Synchronization
        {
            public double RealTime;
            public double MovieFrameTimestamp;
        }

        private readonly IMovieReader reader;
        private readonly Stack<MovieFrame> previousFrames = new Stack<MovieFrame>();

        public IMovie Movie { get; }
        public MoviePlaybackState State { get; private set; }

        private Synchronization? synchronization;
        private double currentTimestamp;
        private MovieFrame currentFrame;
        private double videoSpeed;
        private bool playingReversed;

        public byte[] FrameRawRgba => currentFrame.RgbaData;
        public double FrameTimestamp => currentTimestamp;

        public static readonly double[] MovieSpeeds = Enumerable.Range(0, 7).Select(x => 0.125 * Math.Pow(2, x))
            .ToArray();

        public bool HasTransparency => false;

        public StandardMoviePlayback(IMovie movie)
            : base(ResourceVolatility.Volatile)
        {
            Movie = movie;
            reader = movie.Read();
            videoSpeed = 1.0;
            currentFrame = new MovieFrame
            {
                RgbaData = new byte[GraphicsHelper.AlignedSize(reader.Width, reader.Height)],
                Timestamp = 0
            };
        }

        public override void Dispose()
        {
            base.Dispose();
            reader.Dispose();
        }

        public void Play()
        {
            if (State == MoviePlaybackState.Playing)
                return;
            if (State == MoviePlaybackState.End)
                SeekToTimestamp(playingReversed ? Movie.Duration : 0);
            synchronization = null;
            State = MoviePlaybackState.Playing;
        }

        public void Pause()
        {
            State = MoviePlaybackState.Paused;
        }

        public void Stop()
        {
            // todo: reset reader
            State = MoviePlaybackState.Stopped;
        }

        public void GoToStart()
        {
            SeekToTimestamp(0);
        }

        public void GoToEnd()
        {
            SeekToTimestamp(Movie.Duration);
            while (reader.ReadNextPacket())
            while (reader.FrameQueue.Any())
                currentFrame = reader.FrameQueue.Dequeue();
            State = MoviePlaybackState.End;
            currentTimestamp = Movie.Duration;
        }

        public void PlayFaster()
        {
            if (!(GetVideoSpeed() < GetMaximumMovieSpeed())) return;
            videoSpeed *= 2;
            synchronization = null;
        }

        public void PlaySlower()
        {
            if (!(GetVideoSpeed() > GetMinimumMovieSpeed())) return;
            videoSpeed /= 2;
            synchronization = null;
        }

        public void ReverseDirection()
        {
            /* Todo Every time we wish to seek back and get new stack of reversed samples, we have to jump
               Todo to a new timestamp whose value is a new one, otherwise... it's not worth the try */
            reader.FrameQueue.Clear();
            reader.AudioQueue.Clear();
            synchronization = null;
            if (playingReversed)
            {
                SeekToTimestampInternal(currentTimestamp, out _);
                previousFrames.Clear();
            }
            playingReversed = !playingReversed;
        }

        public void UpdatePlayStatus()
        {
            switch (State)
            {
                case MoviePlaybackState.Paused: // Check whether we wish that pressing pause, may unpause a video
                case MoviePlaybackState.Stopped:
                    Play();
                    break;
                case MoviePlaybackState.End:
                    GoToStart();
                    Play();
                    break;

                case MoviePlaybackState.Playing:
                    Pause();
                    break;
            }
        }

        public void SeekToTimestamp(double timestamp)
        {
            SeekToTimestampInternal(timestamp, out currentTimestamp);
        }

        private void SeekToTimestampInternal(double timestamp, out double newTimestamp)
        {
            reader.FrameQueue.Clear();
            reader.AudioQueue.Clear();
            previousFrames.Clear();
            reader.SeekToTimestamp(timestamp);
            synchronization = null;
            if (TryEnsureFrameQueueNotEmpty())
            {
                currentFrame = reader.FrameQueue.Dequeue();
                newTimestamp = currentFrame.Timestamp;
            }
            else
            {
                // should never happen
                currentFrame = new MovieFrame
                {
                    RgbaData = new byte[GraphicsHelper.AlignedSize(reader.Width, reader.Height)],
                    Timestamp = timestamp
                };
                newTimestamp = Movie.Duration;
            }
        }

        private bool TrySeekToPreviousKeyframe(double targetTimestamp)
        {
            if (targetTimestamp == 0.0)
                return false;
            var offset = 0.0;
            double soughtTimestamp;
            do
            {
                SeekToTimestampInternal(targetTimestamp - offset, out soughtTimestamp);
                offset += 0.1;
            } while (soughtTimestamp >= targetTimestamp);
            return true;
        }

        private static double GetMinimumMovieSpeed()
        {
            return MovieSpeeds[0];
        }

        private static double GetMaximumMovieSpeed()
        {
            return MovieSpeeds[MovieSpeeds.Length - 1];
        }

        public double GetVideoSpeed()
        {
            return videoSpeed;
        }

        public void OnUpdate(FrameTime frameTime)
        {
            switch (State)
            {
                case MoviePlaybackState.Stopped:
                    break;
                case MoviePlaybackState.Playing:
                    if (!synchronization.HasValue)
                    {
                        synchronization = new Synchronization
                        {
                            RealTime = frameTime.TotalSeconds,
                            MovieFrameTimestamp = currentTimestamp
                        };
                    }

                    var realTimePassed = frameTime.TotalSeconds - synchronization.Value.RealTime;
                    var signedSpeed = playingReversed ? -videoSpeed : videoSpeed;
                    currentTimestamp = Math.Max(0, realTimePassed * signedSpeed + synchronization.Value.MovieFrameTimestamp);

                    if (playingReversed)
                    {
                        while (currentFrame.Timestamp > currentTimestamp)
                        {
                            if (previousFrames.Count == 0 && !TryFillFrameStack(currentTimestamp))
                            {
                                ReverseDirection();
                                return;
                            }
                            currentFrame = previousFrames.Pop();
                        }
                    }
                    else
                    {
                        var continueGettingNext = true;
                        do
                        {
                            if (!TryEnsureFrameQueueNotEmpty())
                            {
                                if (currentTimestamp >= Movie.Duration)
                                    State = MoviePlaybackState.End;
                                return;
                            }

                            var nextFrame = reader.FrameQueue.Peek();
                            if (nextFrame.Timestamp <= currentTimestamp)
                                currentFrame = reader.FrameQueue.Dequeue();
                            else
                                continueGettingNext = false;
                        } while (continueGettingNext);
                    }
                    break;
                case MoviePlaybackState.Paused:
                    break;
                case MoviePlaybackState.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /*
         * 
        public byte[] GetAudioBuffer()
        {
            return Reader.GetAudioBuffer();
        }

        public int GetAudioFreq()
        {
            return Reader.GetAudioFreq();
        }

        */
        private bool TryFillFrameStack(double requiredMovieTimestamp)
        {
            if (!TrySeekToPreviousKeyframe(requiredMovieTimestamp))
                return false;
            FillFrameStackUpTo(requiredMovieTimestamp);
            synchronization = null;
            return true;
        }

        private void FillFrameStackUpTo(double requiredMovieTimestamp)
        {
            var frame = currentFrame;
            while (frame.Timestamp <= requiredMovieTimestamp)
            {
                previousFrames.Push(frame);
                if (!TryEnsureFrameQueueNotEmpty())
                    return;
                frame = reader.FrameQueue.Dequeue();
            }
        }

        private bool TryEnsureFrameQueueNotEmpty()
        {
            while (reader.FrameQueue.Count == 0 && reader.ReadNextPacket())
            {
            }
            return reader.FrameQueue.Count != 0;
        }
    }
}