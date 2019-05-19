using System;
using System.Runtime.InteropServices;

namespace Clarity.Native.Linux
{
    [StructLayout(LayoutKind.Sequential)]
    public struct XVisualInfo
    {
        public IntPtr Visual;
        public IntPtr VisualID;
        public int Screen;
        public int Depth;
        public XVisualClass Class;
        public long RedMask;
        public long GreenMask;
        public long blueMask;
        public int ColormapSize;
        public int BitsPerRgb;

        public override string ToString()
        {
            return String.Format("id ({0}), screen ({1}), depth ({2}), class ({3})",
                VisualID, Screen, Depth, Class);
        }
    }
}