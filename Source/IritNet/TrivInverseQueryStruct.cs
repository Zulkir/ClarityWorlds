namespace IritNet
{
    public unsafe struct TrivInverseQueryStruct
    {
        public MvarMVStructPtrArray4 MVScalar;
        public MvarMVStructPtrArray4 MVCopy;
        public fixed double Max[3];
        public fixed double Min[3];
        public MvarPtStruct *UVWPos;
    }
}