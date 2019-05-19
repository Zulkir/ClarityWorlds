namespace IritNet
{
    public unsafe struct IPObjectStructLst
    {
        public IPObjectStruct** PObjList;            /* List of objects. */
        public int ListMaxLen;           /* Maximum number of elements in list. */
    }
}