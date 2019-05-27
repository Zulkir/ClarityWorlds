namespace IritNet
{
    public unsafe struct UserTopoUnstrctGeomStruct
    {
        public  UserTopoUnstrctGeomStruct* Pnext;
        public IPAttributeStruct* Attr;
        public UserTopoUnstrctGeomPtStruct* PtsVec;      /* All the points. */
        public int NumPts;
        public int _NextEntId;
        public IPObjectStruct* CellList;                /* List of all entities. */
        public UserTopoAdjStruct* _AdjList;     /* List of all adjacencies. */
    }
}
