using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Clarity.Common.Numericals;
using Clarity.Engine.Media.Movies;
using FFmpeg.AutoGen;

namespace Clarity.Ext.Video.FFmpeg
{
    public unsafe class FFmpegMovieReader : IMovieReader
    {
        private const AVPixelFormat DestinationPixFmt = AVPixelFormat.AV_PIX_FMT_BGRA;
        private const int ConvertedBufferAlign = 4;

        private readonly IFFmpegMovieReaderIoContextWrapper contextWrapper;
        private readonly AVFormatContext* pFormatContext;
        private readonly AVCodecContext* pVideoCodecContext;
        //private readonly AVCodecContext* pAudioCodecContext;
        private readonly int streamVideoIndex;
        //private readonly int streamAudioIndex;
        private readonly SwsContext* pConvertContext;
        private readonly AVFrame* pConvertedFrame;
        private readonly void* convertedFrameBuffer;
        private readonly byte_ptrArray4 dstData;
        private readonly int_array4 dstLinesize;
        private readonly AVFrame* pDecodedFrame;
        private AVPacket packet;

        public int Width { get; }
        public int Height { get; }
        public double Duration { get; }

        private readonly int videoDataSize;

        public Queue<MovieFrame> FrameQueue { get; }
        public Queue<MovieAudioFrame> AudioQueue { get; }

        public FFmpegMovieReader(Stream fileStream) 
            : this(new FFmpegMovieReaderFileStreamIoContextWrapper(fileStream), "")
        {
        }

        public FFmpegMovieReader(string url)
            : this(new FFmpegMovieReaderDefaultIoContextWrapper(), url)
        {
        }

        public FFmpegMovieReader(IFFmpegMovieReaderIoContextWrapper contextWrapper, string url)
        {
            FrameQueue = new Queue<MovieFrame>();
            AudioQueue = new Queue<MovieAudioFrame>();

            this.contextWrapper = contextWrapper;

            pFormatContext = ffmpeg.avformat_alloc_context();
            contextWrapper.PrepareFormatContext(pFormatContext);
            var pFormatContextLoc = pFormatContext;
            var openInputResult = ffmpeg.avformat_open_input(&pFormatContextLoc, url, null, null);
            if (openInputResult != 0)
                throw new Exception("Could not open format.");

            if (ffmpeg.avformat_find_stream_info(pFormatContext, null) != 0)
                throw new Exception("Could not find stream info");
            streamVideoIndex = GetVideoStreamIndex();
            //streamAudioIndex = GetAudioStreamIndex();
            pVideoCodecContext = pFormatContext->streams[streamVideoIndex]->codec;
            //pAudioCodecContext = pFormatContext->streams[streamAudioIndex]->codec;

            var width = Width = pVideoCodecContext->width;
            var height = Height = pVideoCodecContext->height;
            Duration = (double)pFormatContext->duration / ffmpeg.AV_TIME_BASE;
            var sourcePixFmt = pVideoCodecContext->pix_fmt;
            var videoCodecId = pVideoCodecContext->codec_id;
            //var audioCodecId = pAudioCodecContext->codec_id;
            pConvertContext = ffmpeg.sws_getContext(width, height, sourcePixFmt,
                width, height, DestinationPixFmt,
                ffmpeg.SWS_FAST_BILINEAR, null, null, null);
            if (pConvertContext == null)
                throw new Exception("Could not initialize the conversion context.");

            videoDataSize = GraphicsHelper.AlignedSize(width, height);

            pConvertedFrame = ffmpeg.av_frame_alloc();
            var convertedFrameBufferSize =
                ffmpeg.av_image_get_buffer_size(DestinationPixFmt, width, height, ConvertedBufferAlign);
            convertedFrameBuffer = ffmpeg.av_malloc((ulong)convertedFrameBufferSize);
            dstData = new byte_ptrArray4();
            dstLinesize = new int_array4();

            if (ffmpeg.av_image_fill_arrays(ref dstData, ref dstLinesize, (byte*)convertedFrameBuffer,
                    DestinationPixFmt, width, height, ConvertedBufferAlign) < 0)
                throw new Exception("Failed to setup destination buffer.");

            var pVideoCodec = ffmpeg.avcodec_find_decoder(videoCodecId);
            if (pVideoCodec == null)
                throw new ApplicationException("Unsupported video codec.");
            //var pAudioCodec = ffmpeg.avcodec_find_decoder(audioCodecId);
            //if (pAudioCodec == null)
            //    throw new ApplicationException("Unsupported audio codec.");

            if ((pVideoCodec->capabilities & ffmpeg.AV_CODEC_CAP_TRUNCATED) == ffmpeg.AV_CODEC_CAP_TRUNCATED)
                pVideoCodecContext->flags |= ffmpeg.AV_CODEC_FLAG_TRUNCATED;

            var openVideoCodecResult = ffmpeg.avcodec_open2(pVideoCodecContext, pVideoCodec, null);
            if (openVideoCodecResult < 0)
                throw new Exception($"Could not open video codec: {openVideoCodecResult}");

            //var openAudioCodecResult = ffmpeg.avcodec_open2(pAudioCodecContext, pAudioCodec, null);
            //if (openAudioCodecResult < 0)
            //    throw new Exception($"Could not open audio codec: {openAudioCodecResult}");

            pDecodedFrame = ffmpeg.av_frame_alloc();
            packet = new AVPacket();
            var packetLoc = packet;
            ffmpeg.av_init_packet(&packetLoc);
            packet = packetLoc;
        }

        public void Dispose()
        {
            var packetLoc = packet;
            var pPacket = &packetLoc;
            while (pPacket->side_data_elems > 0)
                ffmpeg.av_packet_unref(pPacket);
            var pDecodedFrameLoc = pDecodedFrame;
            ffmpeg.av_frame_unref(pDecodedFrameLoc);
            ffmpeg.av_frame_free(&pDecodedFrameLoc);
            var pCodecContextLoc = pVideoCodecContext;
            ffmpeg.avcodec_close(pCodecContextLoc);
            ffmpeg.av_free(convertedFrameBuffer);
            var pConvertedFrameLoc = pConvertedFrame;
            ffmpeg.av_frame_unref(pConvertedFrameLoc);
            ffmpeg.av_frame_free(&pConvertedFrameLoc);
            ffmpeg.sws_freeContext(pConvertContext);
            ffmpeg.avformat_free_context(pFormatContext);
            contextWrapper.Dispose();
        }

        public void SeekToTimestamp(double timestampInSeconds)
        {
            var timestamp = (long)(timestampInSeconds * ffmpeg.AV_TIME_BASE);
            var seekResult = ffmpeg.av_seek_frame(pFormatContext, -1, timestamp, ffmpeg.AVSEEK_FLAG_BACKWARD);
            if (seekResult < 0)
                throw new Exception(seekResult.ToString());
        }

        public bool ReadNextPacket()
        {
            var packetLoc = packet;
            var pPacket = &packetLoc;
            var pVideoStream = pFormatContext->streams[streamVideoIndex];
            //var pAudioStream = pFormatContext->streams[streamAudioIndex];
            //    var pAudioStream= pFormatContext->streams[streamAudioIndex];
            //   var pVideoCodecContext = pFormatContext->streams[streamIndex]->codec;

            //var readFullFrame = false;

            // Ignore the return value, which can randomly be -1 for MKVs on last frame without any other issues.
            // todo: send NULL packet at the end of the video or something
            var readFrameRet = ffmpeg.av_read_frame(pFormatContext, pPacket);
            packet = packetLoc;
            if (readFrameRet == ffmpeg.AVERROR_EOF)
            {
                ffmpeg.av_packet_unref(pPacket);
                return false;
            }

            if (pPacket->stream_index == pVideoStream->index)
            {
                var sendPacketResult = ffmpeg.avcodec_send_packet(pVideoCodecContext, pPacket);
                if (sendPacketResult >= 0)
                {
                    while (ffmpeg.avcodec_receive_frame(pVideoCodecContext, pDecodedFrame) >= 0)
                    {
                        ffmpeg.sws_scale(pConvertContext, pDecodedFrame->data, pDecodedFrame->linesize, 0, Height,
                            dstData, dstLinesize);

                        var rgbaData = new byte[videoDataSize];
                        Marshal.Copy((IntPtr)convertedFrameBuffer, rgbaData, 0, rgbaData.Length);
                        var timeStamp = (double)(pDecodedFrame->pts * pVideoStream->time_base.num) / pVideoStream->time_base.den;
                        FrameQueue.Enqueue(new MovieFrame
                        {
                            RgbaData = rgbaData,
                            Timestamp = timeStamp
                        });
                    }
                    ffmpeg.av_frame_unref(pDecodedFrame);
                }
            }
            //else if (pPacket->stream_index == pAudioStream->index)
            //{
            //    var sendPacketResult = ffmpeg.avcodec_send_packet(pAudioCodecContext, pPacket);
            //    if (sendPacketResult >= 0)
            //    {
            //        while (ffmpeg.avcodec_receive_frame(pAudioCodecContext, pDecodedFrame) >= 0)
            //        {
            //            var format = (AVSampleFormat)pDecodedFrame->format;
            //            var planar = ffmpeg.av_sample_fmt_is_planar(format) != 0;
            //            var channels = ffmpeg.av_get_channel_layout_nb_channels(pDecodedFrame->channel_layout);
            //            var planes = planar ? channels : 1;
            //            var bps = ffmpeg.av_get_bytes_per_sample(format);
            //            var planeSize = bps * pDecodedFrame->nb_samples * (planar ? 1 : channels);
            //            var sampleRate = pDecodedFrame->sample_rate;
            //
            //            // todo: correctly process the planar case
            //            var sampleData = new byte[planeSize];
            //            Marshal.Copy((IntPtr)pDecodedFrame->extended_data[0], sampleData, 0, planeSize);
            //            var timeStamp = (double)(pDecodedFrame->pts * pAudioStream->time_base.num) / pAudioStream->time_base.den;
            //            AudioQueue.Enqueue(new MovieAudioFrame
            //            {
            //                SamplesData = sampleData,
            //                Timestamp = timeStamp,
            //                SampleRate = sampleRate,
            //                NumChannels = planar ? 1 : channels,
            //                BitsPerSample = bps
            //            });
            //        }
            //        ffmpeg.av_frame_unref(pDecodedFrame);
            //    }
            //}

            ffmpeg.av_packet_unref(pPacket);

            return true;
        }

        

        private int GetVideoStreamIndex()
        {
            for (var i = 0; i < pFormatContext->nb_streams; i++)
                if (pFormatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                    return i;
            throw new Exception("Could not found video stream");
        }


        //private int GetAudioStreamIndex()
        //{
        //    for (var i = 0; i < pFormatContext->nb_streams; i++)
        //        if (pFormatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
        //            return i;
        //    throw new Exception("Could not found audio stream");
        //}
    }
}