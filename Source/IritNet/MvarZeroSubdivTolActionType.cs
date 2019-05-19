namespace IritNet
{
    public enum MvarZeroSubdivTolActionType
    {
        MVAR_ZERO_ALWAYS_PURGE = 0, /* Return nothing when reaching subdiv tol'. */
        MVAR_ZERO_NEVER_PURGE,            /* Return the domain's center, always. */
        MVAR_ZERO_RETURN_VERIFIED,    /* Return the center if L1 error is small. */
        MVAR_ZERO_NUMERIC_STEP,   /* Try to improve numerically from the center. */
        MVAR_ZERO_NUMERIC_STEP_VERIFIED   /* Improve numerically from the center */
        /* and then verify the L1 error of the answer is small. */
    }
}