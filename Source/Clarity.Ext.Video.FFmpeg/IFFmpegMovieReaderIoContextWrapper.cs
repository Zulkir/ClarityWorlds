using System;
using FFmpeg.AutoGen;

namespace Clarity.Ext.Video.FFmpeg
{
    public unsafe interface IFFmpegMovieReaderIoContextWrapper : IDisposable
    {
        void PrepareFormatContext(AVFormatContext* pFormatContext);
    }
}