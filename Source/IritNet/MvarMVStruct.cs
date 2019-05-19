namespace IritNet
{
    public unsafe struct MvarMVStruct
    {
        public MvarMVStruct *Pnext;
        public IPAttributeStruct *Attr;
        public MvarGeomType GType;
        public MvarPointType PType;
        public int Dim;              /* Number of dimensions in this multi variate. */
        public int* Lengths;               /* Dimensions of mesh size in multi-variate. */
        public int* SubSpaces;    /* SubSpaces[i] = Prod(i = 0, i-1) of Lengths[i]. */
        public int* Orders;                  /* Orders of multi variate (Bspline only). */
        public int* Periodic;            /* Periodicity - valid only for Bspline. */
        public DoublePtrArray20 Points; /* Pointer on each axis vector. */
        public double** KnotVectors;
        public MvarMinMaxType* AuxDomain;            /* Optional to hold MV domain. */
        public void* PAux;                   /* Auxiliary data structure. */
        public void* PAux2;			        /* Auxiliary data structure. */
    }
}