namespace IritNet
{
    public unsafe struct MvarPtStruct
    {
        public  MvarPtStruct* Pnext;
        public  IPAttributeStruct* Attr;
        public int Dim;                                     /* Number of coordinates in Pt. */
        public double* Pt;               /* The coordinates of the multivariate point. */
    }
}
