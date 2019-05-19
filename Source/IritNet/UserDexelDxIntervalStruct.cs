namespace IritNet
{
    public unsafe struct UserDexelDxIntervalStruct
    {
        public UserDexelDxIntervalStruct *Pnext;
        public IPAttributeStruct *Attr;
        public double Start;
        public double End;
        public fixed double NrmlStart[3];
        public fixed double NrmlEnd[3];
        public int StrtShIndx;
        public int EndShIndx;
        public int StrtCutIndx;
        public int EndCutIndx;
    }
}