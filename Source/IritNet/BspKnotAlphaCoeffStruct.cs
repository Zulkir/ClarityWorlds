namespace IritNet
{
    public unsafe struct BspKnotAlphaCoeffStruct
    {
        public int Order, Length, RefLength, Periodic;  /* Dimensions of alpha matrix. */
        public double *Matrix;
        public double *MatrixTransp;
        public double **Rows;                    /* A column of pointers to Matrix rows. */
        public double **RowsTransp;            /* A row of pointers to Matrix columns. */
        public int *ColIndex;        /* A row of indices of first non zero value in col. */
        public int *ColLength;             /* A row of lengths of non zero values in col. */
        public double *_CacheKVT;           /* To compare input/output kvs in cache. */
        public double *_CacheKVt;
    }
}
