using System;
using Clarity.Common.CodingUtilities.Unmanaged;

namespace Clarity.Engine.Resources.RawData
{
    public class RawDataResourceDisposableMap : AutoDisposableBase, IRawDataResourceDisposableMap
    {
        public IntPtr Ptr { get; }
        private readonly RawDataResource dataResource;
        private readonly bool willModify;
        private readonly Action<RawDataResource, bool> onDispose;

        public RawDataResourceDisposableMap(IntPtr ptr, RawDataResource dataResource, bool willModify, Action<RawDataResource, bool> onDispose)
        {
            Ptr = ptr;
            this.dataResource = dataResource;
            this.willModify = willModify;
            this.onDispose = onDispose;
        }

        protected override void Dispose(bool explicitly)
        {
            onDispose(dataResource, willModify);
        }
    }
}