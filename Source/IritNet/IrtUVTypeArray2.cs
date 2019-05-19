using System;

namespace IritNet
{
    public struct IrtUVTypeArray2
    {
        public IrtUVType F0;
        public IrtUVType F1;

        public IrtUVType this[int index]
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