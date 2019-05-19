namespace IritNet
{
    public unsafe struct CagdBlsmAlphaCoeffStruct
    {
        public int Order, Length,
             NewOrder, NewLength, Periodic;/* Dimensions of blossom alpha matrix.*/
        public double *Matrix;
        public double **Rows;                    /* A column of pointers to Matrix rows. */
        public int *ColIndex;        /* A row of indices of first non zero value in col. */
        public int *ColLength;             /* A row of lengths of non zero values in col. */
        public double *KV;            /* Knot sequences before and after the blossom. */
        public double *NewKV;
    }
}
