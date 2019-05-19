namespace IritNet
{
    public unsafe struct UserDexelDxGridStruct
    {
        public UserDexelDxGridType GType;
        public fixed double Origin[2];
        public fixed double Distance[2];
        public fixed int NumDexel[2];
        public UserDexelDxIntervalStruct ***Intrvls;    
    }
}