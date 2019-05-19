using System.Collections.Generic;

namespace Clarity.Common.CodingUtilities.Pools
{
    public class IntegerPool
    {
        private readonly Stack<int> returned;
        private int total;

        public IntegerPool()
        {
            returned = new Stack<int>();
            total = 0;
        }

        public int Allocate()
        {
            if (returned.Count > 0)
                return returned.Pop();
            return total++;
        }

        public void Return(int n)
        {
            returned.Push(n);
        }
    }
}