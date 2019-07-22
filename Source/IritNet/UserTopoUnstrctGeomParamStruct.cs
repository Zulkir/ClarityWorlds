using System;

namespace IritNet
{
    public unsafe struct UserTopoUnstrctGeomParamStruct
    {
        public struct PointSetStruct
        {
            public UserTopoUnstrctGeomPtStruct *PtVec;
            public int PtVecLen;
        }

        public struct IDSetStruct
        {
            public int *IdVec;
            public int IdVecLen;
        }

        public struct AttrParamStructStruct
        {
            public char *AttrName;
            public void *AttrValVec;
            public int AttrValVecLen;
            public UserTopoUnstrctGridAttrType AttrType;
        }

        public int UdId;
        public int UdId2;
        public PointSetStruct PointSet; 
        public IDSetStruct IDSet;
        public IPObjectStruct *Cell;
        public IntPtr FilterCBFunc;
        /* following for attribute setting */
        public AttrParamStructStruct AttrParamStruct;
        public double Eps;
    }
}