namespace IritNet
{
    public unsafe struct CagdCrvStruct
    {
        public  CagdCrvStruct* Pnext;
        public  IPAttributeStruct* Attr;
        public CagdGeomType GType;
        public CagdPointType PType;
        public int Length;           /* Number of control points (== order in Bezier). */
        public int Order;          /* Order of curve (only for B-spline, ignored in Bezier). */
        public int Periodic;                         /* Valid only for B-spline curves. */
        public DoublePtrArray19 Points;    /* Pointer on each axis vector. */
        public double* KnotVector;
    }
}
