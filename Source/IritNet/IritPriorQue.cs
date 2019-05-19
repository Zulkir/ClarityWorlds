namespace IritNet
{
    public unsafe struct IritPriorQue
    {
        public IritPriorQue *Right; /* Pointers to two sons of this node. */
        public IritPriorQue *Left; 
        public void* Data;			     /* Pointers to the data itself. */
    }
}