namespace IritNet
{
    public unsafe struct UserTopoAdjStruct
    {
        public  UserTopoAdjStruct *Pnext;
        public IPAttributeStruct *Attr;
        public UserTopoCellRefStruct *CellRef;
        public UserTopoAdjRelStruct *AdjRelList;
    }
}
