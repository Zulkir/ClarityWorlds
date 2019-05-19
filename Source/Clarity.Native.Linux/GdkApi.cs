using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Clarity.Native.Linux
{
    public static class GdkApi
    {
        /// <summary> Returns the X display of a GdkDisplay. </summary>
        /// <remarks> Display* gdk_x11_display_get_xdisplay(GdkDisplay *display); </remarks>
        /// <param name="gdkDisplay"> The GdkDrawable. </param>
        /// <returns> The X Display of the GdkDisplay. </returns>
        [SuppressUnmanagedCodeSecurity, DllImport("libgdk-x11-2.0.so.0")]
        public static extern IntPtr gdk_x11_display_get_xdisplay(IntPtr gdkDisplay);

        /// <summary> Returns the X resource (window or pixmap) belonging to a GdkDrawable. </summary>
        /// <remarks> XID gdk_x11_drawable_get_xid(GdkDrawable *drawable); </remarks>
        /// <param name="gdkDisplay"> The GdkDrawable. </param>
        /// <returns> The ID of drawable's X resource. </returns>
        [SuppressUnmanagedCodeSecurity, DllImport("libgdk-x11-2.0.so.0")]
        public static extern IntPtr gdk_x11_drawable_get_xid(IntPtr gdkDisplay);
    }
}