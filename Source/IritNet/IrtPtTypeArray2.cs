using System;

namespace IritNet
{
    public struct IrtPtTypeArray2
    {
        public IrtPtType F0;
        public IrtPtType F1;

        public IrtPtType this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return F0;
                    case 1: return F1;
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0: F0 = value; break;
                    case 1: F1 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }
    }
}