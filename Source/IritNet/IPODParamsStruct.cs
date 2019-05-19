namespace IritNet
{
    public unsafe struct IPODParamsStruct
    {
        public IPODParamsStruct* Pnext;
        public byte* Name; /* Name of object that serves as a parameter of this object. */
    }
}