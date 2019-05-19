namespace IritNet
{
    public unsafe struct UserCARefCrvStruct
    {
        public UserCARefCrvStruct *Pnext;
        public UserCACrvStruct *RefCrv;
        public int Inverted;         /* If TRUE the curve is to be considered inverted. */
    }
}