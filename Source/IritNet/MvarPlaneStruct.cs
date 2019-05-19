namespace IritNet
{
    public unsafe struct MvarPlaneStruct
    {
        public  MvarPlaneStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public int Dim;    /* Number of coordinates in Pln (one above space dimension). */
        public double *Pln;               /* The coordinates of the multivariate plane. */
    }
}
