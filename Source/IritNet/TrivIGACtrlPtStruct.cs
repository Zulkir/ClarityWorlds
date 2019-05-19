namespace IritNet
{
    public unsafe struct TrivIGACtrlPtStruct
    {
        public CagdPointType PtType;
        public fixed double Coord[Irit.CAGD_MAX_PT_SIZE];
        public int ID;
    }
}