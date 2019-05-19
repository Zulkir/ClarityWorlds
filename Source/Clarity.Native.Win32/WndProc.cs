using System;

namespace Clarity.Native.Win32
{
    public delegate IntPtr WndProc(IntPtr hWnd, WM msg, IntPtr wParam, IntPtr lParam);
}
