namespace IritNet
{
    public unsafe struct MvarSrfSrfInterCacheDataStruct
    {
        public MvarSrfSrfInterCacheDataStruct *Pnext;
        public int Id1;
        public int Id2;
        public fixed double KnotSpan1[4];
        public fixed double KnotSpan2[4];
        public MvarPolylineStruct *InterRes;
    }
}