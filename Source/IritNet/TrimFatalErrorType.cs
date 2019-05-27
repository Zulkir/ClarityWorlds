﻿namespace IritNet
{
    public enum TrimFatalErrorType
    {
        TRIM_ERR_TRIM_CRV_E2 = 2000,
        TRIM_ERR_BSPLINE_EXPECT,
        TRIM_ERR_BZR_BSP_EXPECT,
        TRIM_ERR_DIR_NOT_CONST_UV,
        TRIM_ERR_ODD_NUM_OF_INTER,
        TRIM_ERR_TCRV_ORIENT,
        TRIM_ERR_INCONSISTENT_CNTRS,
        TRIM_ERR_FAIL_MERGE_TRIM_SEG,
        TRIM_ERR_INVALID_TRIM_SEG,
        TRIM_ERR_INCON_PLGN_CELL,
        TRIM_ERR_TRIM_TOO_COMPLEX,
        TRIM_ERR_TRIMS_NOT_LOOPS,
        TRIM_ERR_LINEAR_TRIM_EXPECT,
        TRIM_ERR_NO_INTERSECTION,
        TRIM_ERR_POWER_NO_SUPPORT,
        TRIM_ERR_UNDEF_SRF,
        TRIM_ERR_TRIM_OPEN_LOOP,
        TRIM_ERR_TRIM_OUT_DOMAIN,
        TRIM_ERR_SINGULAR_TRIM_SEG,

        TRIM_ERR_UNDEFINE_ERR
    }
}