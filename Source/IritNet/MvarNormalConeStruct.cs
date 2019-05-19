namespace IritNet
{
    public unsafe struct MvarNormalConeStruct
    {
        public MvarNormalConeStruct *Pnext;
        public IPAttributeStruct *Attr;
        public MvarVecStruct *ConeAxis;
        public double ConeAngleCosine;
        public fixed double AxisMinMax[2];
    }
}