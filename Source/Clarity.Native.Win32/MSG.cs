using System;
using System.Runtime.InteropServices;

namespace Clarity.Native.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr HWnd;
        public WM Message;
        public IntPtr WParam;
        public IntPtr LParam;
        public uint Time;
        public POINT Point;

        public override string ToString()
        {
            return String.Format("msg=0x{0:x} ({1}) hwnd=0x{2:x} wparam=0x{3:x} lparam=0x{4:x} pt={5}", (int)Message, Message.ToString(), HWnd.ToInt32(), WParam.ToInt32(), LParam.ToInt32(), Point.ToString());
        }
    }
}
