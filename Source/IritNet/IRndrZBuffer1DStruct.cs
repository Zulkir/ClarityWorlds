namespace IritNet
{
    public unsafe struct IRndrZBuffer1DStruct
    {
        public double *ZBuf;
        public IRndrZBufferCmpType ZBufCmp;
        public double XMin, XMax, Dx;		     /* Real range of X in Zbuffer. */
        public int Size;					   /* Width of 1D Z buffer. */
        public int *ZBufLnSegIndex;/* Place to keep scan converted ZBuf line indicess. */
        public int CurrentLine;	    /* Actual number of lines kept in Lines vector. */
        public int MaxNumOfLines;     /* Maximal number of lines we can hold in Lines. */
        public IRndrZBuffer1DLineStruct *Lines;     /* Keeps all scan converted lines. */
    }
}