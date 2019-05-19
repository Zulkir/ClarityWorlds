namespace IritNet
{
    public unsafe struct VMdlSliceLclInfoStruct
    {
        public VMdlSlicerParamDataStruct *Buffer;
        public fixed int Size[2];
        public int Num;

        public double Res;
        public fixed double CellSize[2];
        public fixed double InvCellSize[2];

        public fixed double PMax[3];
        public fixed double PMin[3];
        public fixed double InvRange[3];

        public double z;
    }
}