using System;
using System.IO;

namespace Clarity.Common.Infra.Files
{
    public class MemoryFileSystemWriteStream : Stream
    {
        private readonly MemoryStream internalStream;
        private readonly Action<byte[]> onFlush;

        public MemoryFileSystemWriteStream(Action<byte[]> onFlush)
        {
            this.onFlush = onFlush;
            internalStream = new MemoryStream();
        }

        public override void Flush()
        {
            internalStream.Flush();
            onFlush(internalStream.GetBuffer());
        }

        protected override void Dispose(bool disposing)
        {
            Flush();
            base.Dispose(disposing);
        }

        public override long Seek(long offset, SeekOrigin origin) => internalStream.Seek(offset, origin);
        public override void SetLength(long value) => internalStream.SetLength(value);
        public override int Read(byte[] buffer, int offset, int count) => internalStream.Read(buffer, offset, count);
        public override void Write(byte[] buffer, int offset, int count) => internalStream.Write(buffer, offset, count);

        public override bool CanRead => internalStream.CanRead;
        public override bool CanSeek => internalStream.CanSeek;
        public override bool CanWrite => internalStream.CanWrite;
        public override long Length => internalStream.Length;
        public override long Position
        {
            get => internalStream.Position;
            set => internalStream.Position = value;
        }
    }
}