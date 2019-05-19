using System;

namespace Clarity.Native.Win32
{
    public delegate void TimerProc(IntPtr hWnd, uint uMsg, IntPtr nIDEvent, uint dwTime);
}
