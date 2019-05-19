namespace IritNet
{
    public unsafe struct IPInstanceStruct
    {
        public  IPInstanceStruct *Pnext;                        /* To next in chain. */
        public  IPAttributeStruct *Attr;
        public  IPObjectStruct *PRef;/* Reference to object this is its instance. */
        public byte *Name;                             /* Name of object this is its instance. */
        public IrtHmgnMatType Mat;          /* Transformation from Object Name to this object. */
    }
}
