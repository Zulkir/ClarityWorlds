namespace IritNet
{
    public unsafe struct FastAllocStruct
    {
        public byte* NextAlloc;                        /* Pointer to next allocation. */
        public byte* End;     /* Pointer beyond the last allocation in current block. */
        public int Offset;             /* Allocation size (aligned type size). */
        public int Count;    /* Number of allocations made so far (statistic). */
        public BlkStruct* CurrBlk;              /* Pointer to current block descriptor. */
        public BlkStruct* FirstBlk;               /* Pointer to first block descriptor. */
        public int TypeSize;            /* Copies of initialization variables. */
        public int BlkSize;
        public int AllgnBits;
        public int Verbose;
    }
}