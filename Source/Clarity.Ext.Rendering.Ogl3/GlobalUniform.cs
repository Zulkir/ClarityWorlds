using System.Runtime.InteropServices;

namespace Clarity.Ext.Rendering.Ogl3
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GlobalUniform
    {
        public int ScreenWidth;
        public int ScreenHeight;
        public float Time;
    }
}