using System;

namespace IritNet
{
    public unsafe struct MiscListStruct
    {
        public MiscListNodeStruct* Head, Tail;
        public IntPtr CopyFunc;
        public IntPtr FreeFunc;
        public IntPtr CompFunc;
    }
}