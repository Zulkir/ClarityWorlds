namespace IritNet
{
    public unsafe struct UserTopoAdjRelStruct
    {
        public  UserTopoAdjRelStruct *Pnext;
        public IPAttributeStruct *Attr;
        public UserTopoCellRefStruct *CellRef;
        public UserTopoAdjRelType AdjType;
    }
}
