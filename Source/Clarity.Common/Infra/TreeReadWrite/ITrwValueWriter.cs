namespace Clarity.Common.Infra.TreeReadWrite
{
    public interface ITrwValueWriter
    {
        void Null();
        void String(string val);
        void SInt8(sbyte val);
        void UInt8(byte val);
        void SInt16(short val);
        void UInt16(ushort val);
        void SInt32(int val);
        void UInt32(uint val);
        void SInt64(long val);
        void UInt64(ulong val);
        void Float32(float val);
        void Float64(double val);
        void Bool(bool val);
    }
}