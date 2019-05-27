namespace IritNet
{
    public unsafe struct MvarMatlabEqStruct
    {
        public int NumOfMonoms;                     /* Number of monomials in the equation. */
        public double* CoeffArr;                     /* The coeffs of each monomial. */
        public int* PowersMat;    /* The power of variable j (col) in monomial i (row). */
        public int* MaxPowers;          /* Maximal power of each variable in the equation. */
    }
}
