namespace IritNet
{
    public unsafe struct MvarZeroPrblmStruct
    {
        public struct Union
        {
            public MvarMVStruct** MVs;
            public MvarExprTreeEqnsStruct* Eqns { get { return (MvarExprTreeEqnsStruct*)MVs; } set { MVs = (MvarMVStruct**)value; } }
        }

        public MvarZeroPrblmStruct *Pnext;
        public IPAttributeStruct *Attr;
        public Union U;
        public MvarZrSlvrRepresentationType ActiveRepresentation;
        public MvarConstraintType* Constraints;
        public int NumOfConstraints;
        public int NumOfZeroConstraints;
        public double SubdivTol;
        public double NumericTol;
        public double StepTol;
        public MvarMVZR1DAuxStruct * AS;
        public MvarZeroPrblmIntrnlStruct * _Internal;
    }
}