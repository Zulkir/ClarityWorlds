using System;

namespace IritNet
{
    public unsafe struct MdlIntersectionCBStruct
    {
        public IntPtr InterCBFunc;
        public IntPtr PreInterCBFunc;
        public void* InterCBData;
        public IntPtr PostInterCBFunc;
    }
}
