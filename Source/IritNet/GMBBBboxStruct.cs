namespace IritNet
{
    public unsafe struct GMBBBboxStruct
    {
        public fixed double Min[Irit.GM_BBOX_MAX_DIM];
        public fixed double Max[Irit.GM_BBOX_MAX_DIM];
        public int Dim;                  /* Actual number of valid dimensions in bbox. */
    }
}