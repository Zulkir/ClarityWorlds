namespace IritNet
{
    public unsafe struct UserCARegionStruct
    {
        public UserCARegionStruct *Pnext;
        public UserCARefCrvStruct *RefCrvs;
        public UserCAObjType Type;
        public double Area;
        public int ContainedIn;
    }
}