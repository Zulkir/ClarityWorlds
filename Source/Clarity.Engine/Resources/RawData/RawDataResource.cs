using System;
using System.IO;
using System.Threading;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Unmanaged;
using PtrMagic;

namespace Clarity.Engine.Resources.RawData
{
    public unsafe class RawDataResource : ResourceBase, IRawDataResource
    {
        public int Size { get; }
        private readonly Func<Stream> getStream;
        private readonly IUnmanagedArray permanentArray;
        private IUnmanagedArray temporaryArray;
        private int mapCount;

        public RawDataResource(ResourceVolatility volatility, Func<Stream> getStream, int size)
            : base(volatility)
        {
            Size = size;
            this.getStream = getStream;
        }

        public RawDataResource(ResourceVolatility volatility, IntPtr data, int size) 
            : base(volatility)
        {
            Size = size;
            permanentArray = new UnmanagedArray(size);
            PtrHelper.CopyBulk((byte*)permanentArray.Data, (byte*)data, size);
            getStream = () => new UnmanagedMemoryStream((byte*)permanentArray.Data, size);
        }

        public RawDataResource(ResourceVolatility volatility, int size)
            : base(volatility)
        {
            Size = size;
            permanentArray = new UnmanagedArray(size);
            getStream = () => new UnmanagedMemoryStream((byte*)permanentArray.Data, size);
        }

        public override void Dispose()
        {
            base.Dispose();
            permanentArray?.Dispose();
        }

        public Stream Open() => getStream();

        public IntPtr Map()
        {
            if (permanentArray != null)
                return permanentArray.Data;
            Interlocked.Increment(ref mapCount);
            if (temporaryArray != null)
                return temporaryArray.Data;
            temporaryArray = new UnmanagedArray(Size);
            var readStream = new UnmanagedMemoryStream((byte*)temporaryArray.Data, Size);
            using (var writeStream = getStream())
                writeStream.CopyTo(readStream);
            return temporaryArray.Data;
        }

        public void Unmap(bool wasModified)
        {
            if (permanentArray != null)
            {
                if (wasModified)
                    OnModified(null);
                return;
            }
            if (wasModified)
                throw new InvalidOperationException("Was trying to modify a stream-based RawDataResource");
            Interlocked.Decrement(ref mapCount);
            if (mapCount != 0)
                return;
            temporaryArray?.Dispose();
            temporaryArray = null;
        }

        public IRawDataResourceDisposableMap MapToDisposable(bool willModify)
        {
            var ptr = Map();
            return new RawDataResourceDisposableMap(ptr, this, willModify, (x, m) => x.Unmap(m));
        }
    }
}