using JitsuGen.Core;

namespace JitsuGen.InfrastructureTest
{
    [JitsuGen(typeof(VerboseWrapperGenerator))]
    public interface ICalculator
    {
        int Memory { get; set; }

        int Add(int x, int y);
        int Subtract(int x, int y);
        int Multiply(int x, int y);
        int Divide(int x, int y);
    }
}