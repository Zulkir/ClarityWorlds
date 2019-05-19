namespace IritNet
{
    public unsafe struct CagdSrfStruct
    {
        public CagdSrfStruct *Pnext;
        public IPAttributeStruct *Attr;
        public CagdGeomType GType;
        public CagdPointType PType;
        public int ULength, VLength;   /* Mesh size in the tensor product surface. */
        public int UOrder, VOrder; /* Order in tensor product surface (B-spline only). */
        public int UPeriodic, VPeriodic;    /* Valid only for B-spline surfaces. */
        public DoublePtrArray19 Points;    /* Pointer on each axis vector. */
        public double* UKnotVector, VKnotVector;
        public void* PAux;                        /* Used internally - do not touch. */
    }
}