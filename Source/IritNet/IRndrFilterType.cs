namespace IritNet
{
    public unsafe struct IRndrFilterType
    {
        public int SuperSize;
        public double** FilterData;
        public byte* Name;
        public double TotalWeight;
    }
}