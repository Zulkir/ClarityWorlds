namespace IritNet
{
    public unsafe struct IritHashTableStruct
    {
        public double MinKeyVal, MaxKeyVal, DKey, KeyEps;
        public IritHashElementStruct **Vec;
        public int VecSize;
    }
}
