namespace IritNet
{
    public unsafe struct IPODDependsStruct
    {
        public IPODDependsStruct* Pnext;
        public byte* Name;                /* Name of object that depends on us. */
    }
}