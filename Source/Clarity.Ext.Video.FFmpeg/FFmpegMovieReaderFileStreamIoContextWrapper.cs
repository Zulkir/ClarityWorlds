using System;
using System.IO;
using System.Runtime.InteropServices;
using FFmpeg.AutoGen;

namespace Clarity.Ext.Video.FFmpeg
{
    public unsafe class FFmpegMovieReaderFileStreamIoContextWrapper : IFFmpegMovieReaderIoContextWrapper
    {
        private const int ReadBufferLength = 32 * 1024 * 1024;

        private readonly Stream fileDataStream;

        private readonly void* pReadBuffer;
        private readonly byte[] managedReadBuffer;
        private readonly avio_alloc_context_read_packet ReadPacketDelegate;
        private readonly avio_alloc_context_write_packet WritePacketDelegate = (a, b, c) => 0;
        private readonly avio_alloc_context_seek SeekPacketDelegate;
        private readonly AVIOContext* pIoContext;

        private volatile bool disposing;

        public FFmpegMovieReaderFileStreamIoContextWrapper(Stream fileStream)
        {
            fileDataStream = fileStream;
            managedReadBuffer = new byte[ReadBufferLength];
            pReadBuffer = ffmpeg.av_malloc(ReadBufferLength);
            ReadPacketDelegate = ReadPacket;
            SeekPacketDelegate = Seek;
            pIoContext = ffmpeg.avio_alloc_context(
                (byte*)pReadBuffer,
                ReadBufferLength,
                0,
                pReadBuffer,
                ReadPacketDelegate,
                WritePacketDelegate,
                SeekPacketDelegate);
            pIoContext->direct = 1;
        }

        public void PrepareFormatContext(AVFormatContext* pFormatContext)
        {
            pFormatContext->pb = pIoContext;
            pFormatContext->flags |= ffmpeg.AVFMT_FLAG_CUSTOM_IO;
        }

        public void Dispose()
        {
            ffmpeg.av_free(pIoContext->buffer);
            ffmpeg.av_free(pIoContext);
            fileDataStream.Dispose();
        }

        private int ReadPacket(void* ptr, byte* buffer, int bufferSize)
        {
            if (disposing)
                return 0;
            var bytesToCopy = (int)Math.Min(Math.Min(bufferSize, managedReadBuffer.Length), fileDataStream.Length - fileDataStream.Position);
            var bytesCopied = fileDataStream.Read(managedReadBuffer, 0, bytesToCopy);
            Marshal.Copy(managedReadBuffer, 0, (IntPtr)buffer, bytesCopied);
            return bytesToCopy;
        }

        private long Seek(void* opaque, long pos, int whence)
        {
            if (disposing)
                return -1;

            // todo: make this work
            whence &= ~ffmpeg.AVSEEK_FORCE;
            const int SEEK_SET = 0;
            const int SEEK_CUR = 1;
            const int SEEK_END = 2;
            const int SEEK_SIZE = 65536;

            int newPos;
            switch (whence)
            {
                case SEEK_SET:
                    newPos = (int)pos;
                    break;
                case SEEK_CUR:
                    newPos = (int)(fileDataStream.Position + pos);
                    break;
                case SEEK_END:
                    newPos = (int)(fileDataStream.Length + pos);
                    break;
                case SEEK_SIZE:
                    return fileDataStream.Length;
                default:
                    return -1;
            }

            if (newPos > fileDataStream.Length || newPos < 0)
                return -1;
            fileDataStream.Position = newPos;
            return (int)pos;
        }
    }
}