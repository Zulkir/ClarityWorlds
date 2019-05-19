using System;

namespace IritNet
{
    public unsafe struct UserTopoUnstrctGeomParamStruct
    {
        public struct PointSetStruct
        {
            UserTopoUnstrctGeomPtStruct *PtVec;
            int PtVecLen;
        }

        public struct IDSetStruct
        {
            int *IdVec;
            int IdVecLen;
        }

        public struct AttrParamStructStruct
        {
            char *AttrName;
            void *AttrValVec;
            int AttrValVecLen;
            UserTopoUnstrctGridAttrType AttrType;
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