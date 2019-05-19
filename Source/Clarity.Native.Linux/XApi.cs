using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Clarity.Native.Linux
{
    public static class XApi
    {
        [SuppressUnmanagedCodeSecurity, DllImport("libX11.so.6")]
        public static extern void XFree(IntPtr handle);

        [SuppressUnmanagedCodeSecurity, DllImport("libX11.so.6")]
        public static extern IntPtr XGetVisualInfo(IntPtr display, IntPtr vinfo_mask, ref XVisualInfo template, out int nitems);
    }
}