namespace IritNet
{
    public unsafe struct TrivTVStruct
    {
        public  TrivTVStruct* Pnext;
        public  IPAttributeStruct* Attr;
        public TrivGeomType GType;
        public CagdPointType PType;
        public int ULength, VLength, WLength;/* Mesh size in tri-variate tensor product.*/
        public int UVPlane;          /* Should equal ULength*  VLength for fast access. */
        public int UOrder, VOrder, WOrder;      /* Order in trivariate (B-spline only). */
        public int UPeriodic, VPeriodic, WPeriodic;   /* Valid only for B-spline. */
        public DoublePtrArray19 Points;     /* Pointer on each axis vector. */
        public double* UKnotVector, VKnotVector, WKnotVector;
    }
}
