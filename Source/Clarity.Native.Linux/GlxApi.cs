using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Clarity.Native.Linux
{
    public static class GlxApi
    {
        public const int GLX_NONE = 0;
        public const int GLX_USE_GL = 1;
        public const int GLX_BUFFER_SIZE = 2;
        public const int GLX_LEVEL = 3;
        public const int GLX_RGBA = 4;
        public const int GLX_DOUBLEBUFFER = 5;
        public const int GLX_STEREO = 6;
        public const int GLX_AUX_BUFFERS = 7;
        public const int GLX_RED_SIZE = 8;
        public const int GLX_GREEN_SIZE = 9;
        public const int GLX_BLUE_SIZE = 10;
        public const int GLX_ALPHA_SIZE = 11;
        public const int GLX_DEPTH_SIZE = 12;
        public const int GLX_STENCIL_SIZE = 13;
        public const int GLX_ACCUM_RED_SIZE = 14;
        public const int GLX_ACCUM_GREEN_SIZE = 15;
        public const int GLX_ACCUM_BLUE_SIZE = 16;
        public const int GLX_ACCUM_ALPHA_SIZE = 17;

        [SuppressUnmanagedCodeSecurity, DllImport("libGL.so.1")]
        public static extern IntPtr glXChooseVisual(IntPtr display, int screen, int[] attr);
    }
}