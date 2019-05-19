namespace IritNet
{
    public unsafe struct UserMicroImplicitParamStruct
    {
        public fixed int NumCells[Irit.USER_MACRO_MAX_DIM];
        public fixed int Orders[Irit.USER_MACRO_MAX_DIM];
        public fixed int NumCPInTile[Irit.USER_MACRO_MAX_DIM];
        public double MinCPValue;
        public double MaxCPValue;
        public int IsC1;
    }
}