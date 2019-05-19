namespace IritNet
{
    public unsafe struct TrivTriangleStruct
    {
        public struct TStruct
        {
            public IrtPtType Pt;
            public IrtVecType Nrml;
            public TrivUVWType UVW;
        }

        public struct TStructArray3
        {
            public TStruct T0;
            public TStruct T1;
            public TStruct T2;

            public TStruct this[int index]
            {
                get
                {
                    var loc = this;
                    return ((TStruct*)&loc)[index];
                }
                set
                {
                    var loc = this;
                    ((TStruct*)&loc)[index] = value;
                    this = loc;
                }
            }
        }

        public TrivTriangleStruct *Pnext;
        public IPAttributeStruct *Attr;
        public TStructArray3 T;
    }
}