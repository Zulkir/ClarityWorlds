namespace IritNet
{
    public unsafe struct MiscISCFreeMemoryStruct
    {
        public MiscISCFreeMemoryStruct *PNext;
        public void *MemoryToFree;
    }
}