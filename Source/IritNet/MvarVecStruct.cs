namespace IritNet
{
    public unsafe struct MvarVecStruct
    {
        public  MvarVecStruct* Pnext;
        public  IPAttributeStruct* Attr;
        public int Dim;                                    /* Number of coordinates in Vec. */
        public double* Vec;              /* The coordinates of the multivariate vector. */
    }
}
