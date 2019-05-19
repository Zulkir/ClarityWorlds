using System.Runtime.InteropServices;

namespace Clarity.Ext.Format.Itd 
{
    public static unsafe class IritStrings
    {
        public static byte* uvvals { get; } = (byte*)Marshal.StringToHGlobalAnsi("uvvals");
        public static byte* transp { get; } = (byte*)Marshal.StringToHGlobalAnsi("transp");
    }
}