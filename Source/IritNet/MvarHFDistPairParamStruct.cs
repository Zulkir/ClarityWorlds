namespace IritNet
{
    public unsafe struct MvarHFDistPairParamStruct
    {
        public  MvarHFDistPairParamStruct *Pnext;
        public MvarHFDistParamStruct Param1, Param2;         /* Param. info of the pair. */
        public double Dist;                     /* Euclidean distance at this location. */
    }
}
