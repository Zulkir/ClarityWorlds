namespace IritNet
{
    public unsafe struct MvarHFDistParamStruct
    {
        public int NumOfParams;  /* Number of locations where the Hausdorff dist holds. */
        public int ManifoldDim;                        /* 1 for curves, 2 for surfaces. */
        public DoubleArray3 T { get { var loc = UV; return *(DoubleArray3*)&loc; } set { var loc = UV; *(DoubleArray3*)&loc = value; UV = loc; } }
        public IrtUVTypeArray3 UV;
    }
}