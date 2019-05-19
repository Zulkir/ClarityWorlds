namespace IritNet
{
    public struct IRndrMatrixContextStruct
    {
        public IrtHmgnMatType TotalViewMat;             /* Cumulative matrix of viewing */
        /* transformations.             */
        public IrtHmgnMatType ViewInvMat;             /* Inverse of accumulated matrix. */
        public IrtPtType Viewer;          /* Viewer position (perspective) or direction */
        /* (orthographics).                           */
        public IrtHmgnMatType ViewMat;
        public IrtHmgnMatType PrspMat;
        public IrtHmgnMatType TransMat;
        public IrtHmgnMatType ScreenMat;
        public int ParallelProjection;
    }
}