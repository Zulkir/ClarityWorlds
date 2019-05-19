namespace IritNet
{
    public enum MdlFatalErrorType
    {
        MDL_ERR_NO_ERROR = 0,

        MDL_ERR_PTR_REF = 1000,
        MDL_ERR_TSEG_NO_SRF,
        MDL_ERR_BOOL_MERGE_FAIL,
        MDL_ERR_TSEG_NOT_FOUND,
        MDL_ERR_EXPECTED_LINEAR_TSEG,
        MDL_ERR_TSRF_NOT_FOUND,
        MDL_ERR_FP_ERROR,
        MDL_ERR_BOOL_DISJOINT,
        MDL_ERR_BOOL_GET_REF,
        MDL_ERR_BOOL_CLASSIFY_FAIL,
        MDL_ERR_BOOL_UVMATCH_FAIL,

        MDL_ERR_UNDEFINE_ERR
    }
}