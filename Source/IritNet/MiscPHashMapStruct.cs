namespace IritNet
{
    public unsafe struct MiscPHashMapStruct
    {
        public MiscPHBucketData **HashArray; /* Array of pointers to MiscPHBucketData. */
        public int HashArraySize;
    }
}