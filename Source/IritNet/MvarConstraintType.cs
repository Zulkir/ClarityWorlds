namespace IritNet
{
    public enum MvarConstraintType
    {
        MVAR_CNSTRNT_ZERO = 1320,
        MVAR_CNSTRNT_ZERO_SUBDIV,    /* Examine zeros during subdiv. stage only. */
        MVAR_CNSTRNT_POSITIVE,  /* Examine positivity during subdiv. stage only. */
        MVAR_CNSTRNT_NEGATIVE   /* Examine negativity during subdiv. stage only. */
    }
}