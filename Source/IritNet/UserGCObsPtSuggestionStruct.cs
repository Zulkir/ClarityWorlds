namespace IritNet
{
    public unsafe struct UserGCObsPtSuggestionStruct
    {
        public IrtPtType ObsPt; /* If it's USER_GC_INF_VEC then the ObsPt is at         */
             /* infinity.                                            */
        public IrtVecType Direction;
    }
}
