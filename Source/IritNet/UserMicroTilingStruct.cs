using System;

namespace IritNet
{
    public unsafe struct UserMicroTilingStruct
    {
        public int Dim;
        public fixed int NumCells[Irit.USER_MACRO_MAX_DIM];
        public fixed int Order[Irit.USER_MACRO_MAX_DIM];
        public fixed int NumCP[Irit.USER_MACRO_MAX_DIM];
        public fixed int TotalNumCP[Irit.USER_MACRO_MAX_DIM];
        public IntPtr CBValueFunc;
        public MvarMVStruct **CellMvars;
        public double *CtPnts;
        public double MinCPValue;
        public double MaxCPValue;
        public double Capping;
        public int ShellBits;
    }
}