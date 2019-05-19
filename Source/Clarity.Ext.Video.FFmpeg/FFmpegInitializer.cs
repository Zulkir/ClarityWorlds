using System;
using System.Runtime.InteropServices;
using Clarity.Core.AppCore.Logging;
using FFmpeg.AutoGen;

namespace Clarity.Ext.Video.FFmpeg
{
    public unsafe class FFmpegInitializer
    {
        private static readonly av_log_set_callback_callback LogCallbackDelegate = LogCallback;
        private bool initialized;
        
        public void EnsureInitialized()
        {
            if (initialized)
                return;
            ffmpeg.av_register_all();
            ffmpeg.avcodec_register_all();
            ffmpeg.av_log_set_callback(LogCallbackDelegate);
            initialized = true;
        }

        private static void LogCallback(void* ptr, int level, string format, byte* vl)
        {
            if (level > ffmpeg.av_log_get_level())
                return;

            var lineData = new byte[1024];
            fixed (byte* pLineData = lineData)
            {
                var printPrefix = 1;
                ffmpeg.av_log_format_line(ptr, level, format, vl, pLineData, lineData.Length, &printPrefix);
                var line = Marshal.PtrToStringAnsi((IntPtr)pLineData);
                Log.Write(LogMessageType.Info, line);
            }
        }
    }
}