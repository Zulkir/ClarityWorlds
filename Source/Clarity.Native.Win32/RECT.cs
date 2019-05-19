using System.Runtime.InteropServices;

namespace Clarity.Native.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int Width { get { return right - left; } }
        public int Height { get { return bottom - top; } }
    }
}
