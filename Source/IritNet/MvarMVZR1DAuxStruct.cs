namespace IritNet
{
    public unsafe struct MvarMVZR1DAuxStruct
    {
        public MvarVecStruct** OrthoBasis;
        public MvarVecStruct* TempVec;
        public MvarVecStruct* CorrVec;
        public double* MinDmn;
        public double* MaxDmn;
        public double* MinDmn2;
        public double* MaxDmn2;
        public MvarVecStruct** GradVecs;
        public MvarVecStruct* SITTempVec;/* SIT = StepInTangentVector+CorrStep related.*/
        public MvarVecStruct* SITTanVec;
        public double* A;
        public double* x;
        public double* b;
        public double* bCopy;
        public MvarVecStruct** TempList;
        public MvarConstraintType* Constraints;
        public int NumOfMVs;
    }
}