using System.Runtime.InteropServices;

namespace Clarity.Native.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return "Point {" + X.ToString() + ", " + Y.ToString() + ")";
        }
    }
}
