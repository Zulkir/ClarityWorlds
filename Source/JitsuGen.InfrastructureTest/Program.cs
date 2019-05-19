using System;
using JitsuGen.Core;
using JitsuGen.Output;

namespace JitsuGen.InfrastructureTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            try
            {
                GenOutput.FillDomain(GenDomain.Static);
                var calcType = GenDomain.Static.GetGeneratedType(typeof(ICalculator));
                var calc = (ICalculator)Activator.CreateInstance(calcType, new Calculator());
                Console.WriteLine(calc.Add(1, 2));
                Console.WriteLine(calc.Multiply(3, 4));
                calc.Memory = 123;
                Console.WriteLine(calc.Memory);
                // todo: test generics and ref/out
            }
            catch (Exception e)
            {
                Console.WriteLine($"FAIL: {e.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
