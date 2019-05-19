using System;

namespace IritNet
{
    public unsafe struct UserTopoUnstrctGeomReturnStruct
    {
        public int UdId;
        public int *CloneMap;
        public int *RealIDMap;
        public int *CellIDVec;
        public void **AttrValVec;
        public int Success;
        public IPObjectStruct *Cell;
        public int Id;
        public int Length;
        public int NumPts;
        public IntPtr FilterCBFunc;
    }
}
