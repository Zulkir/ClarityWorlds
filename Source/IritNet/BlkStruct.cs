namespace IritNet
{
    public unsafe struct BlkStruct
    {
        public byte* Bytes;                       /* Pointer to allocated raw memory. */
        public BlkStruct *Next;             /* Pointer to next block descriptor. */
    }
}