namespace IritNet
{
    public unsafe struct IPAttributeStruct
    {
        public struct Union
        {
            public struct VecStruct
            {
                public double *Coord;
                public int Len;
            }

            public byte *Str { get { var loc = this; return *(byte**)&loc; } set { var loc = this; *(byte**)&loc = value; this = loc; } }
            public int I { get { var loc = this; return *(int*)&loc; } set { var loc = this; *(int*)&loc = value; this = loc; } }
            public double R { get { var loc = this; return *(double*)&loc; } set { var loc = this; *(double*)&loc = value; this = loc; } }
            public VecStruct Vec;
            public DoubleArray2 UV { get { var loc = this; return *(DoubleArray2*)&loc; } set { var loc = this; *(DoubleArray2*)&loc = value; this = loc; } }
            public IPObjectStruct *PObj { get { var loc = this; return *(IPObjectStruct**)&loc; } set { var loc = this; *(IPObjectStruct**)&loc = value; this = loc; } }
            public void* Ptr { get { var loc = this; return *(void**)&loc; } set { var loc = this; *(void**)&loc = value; this = loc; } }
            public void* RefPtr { get { var loc = this; return *(void**)&loc; } set { var loc = this; *(void**)&loc = value; this = loc; } }
        }

        public IPAttributeStruct* Pnext;
        public IPAttributeType Type;
        public Union U;
        public int _AttribNum;
    }
}