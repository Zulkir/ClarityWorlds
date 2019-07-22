namespace IritNet
{
    public unsafe struct MvarExprTreeEqnsStruct
    {
        public MvarExprTreeStruct** Eqns;                          /* The equations to solve. */
        public int NumEqns, NumZeroEqns, NumZeroSubdivEqns;
        public MvarExprTreeStruct** CommonExprs;       /* The common expressions found. */
        public int NumCommonExprs, MaxNumCommonExprs;
        public MvarConstraintType* ConstraintTypes;
    }
}
