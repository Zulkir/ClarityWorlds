namespace JitsuGen.InfrastructureTest
{
    public class Calculator : ICalculator
    {
        public int Memory { get; set; }
        public int Add(int x, int y) => x + y;
        public int Subtract(int x, int y) => x - y;
        public int Multiply(int x, int y) => x * y;
        public int Divide(int x, int y) => x / y;
    }
}