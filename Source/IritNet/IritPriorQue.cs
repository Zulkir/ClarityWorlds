namespace IritNet
{
    public unsafe struct IritPriorQue
    {
        public  IritPriorQue* Right, Left; /* Pointers to two sons of this node. */
        public void*  Data;                             /* Pointers to the data itself. */
    }
}
