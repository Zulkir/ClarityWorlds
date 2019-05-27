namespace Clarity.Common.Infra.TreeReadWrite
{
    public interface ITrwValueWriter
    {
        void Null();
        void String(string val);
        void SInt32(int val);
        void Float32(float val);
        void Float64(double val);
        void Bool(bool val);
    }
}