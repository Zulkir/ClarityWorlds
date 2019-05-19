namespace IritNet
{
    public unsafe struct MvarTriangleStruct
    {
        public MvarTriangleStruct *Pnext;
        public int Dim;
        public DoublePtrArray3 Vrtcs;
        public DoublePtrArray3 Nrmls;
    }
}