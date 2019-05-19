namespace IritNet
{
    public unsafe struct IRndrZListStruct
    {
        public IRndrZPointStruct First;/* Head of linked list, contains nearest z-point.*/
        public int Stencil;
    }
}