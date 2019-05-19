namespace IritNet
{
    public unsafe struct TrngTriangSrfStruct
    {
        public  TrngTriangSrfStruct *Pnext;
        public  IPAttributeStruct *Attr;
        public TrngGeomType GType;
        public CagdPointType PType;
        public int Length;                    /* Mesh size (length of edge of triangular mesh. */
        public int Order;                      /* Order of triangular surface (Bspline only). */
        public DoublePtrArray19 Points;     /* Pointer on each axis vector. */
        public double *KnotVector;
    }
}
