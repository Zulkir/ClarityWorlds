﻿namespace IritNet
{
    public enum MvarFatalErrorType
    {
        MVAR_ERR_DIR_NOT_VALID,
        MVAR_ERR_UNDEF_CRV,
        MVAR_ERR_UNDEF_SRF,
        MVAR_ERR_UNDEF_MVAR,
        MVAR_ERR_UNDEF_GEOM,
        MVAR_ERR_GEOM_NO_SUPPORT,
        MVAR_ERR_PERIODIC_NO_SUPPORT,
        MVAR_ERR_RATIONAL_NO_SUPPORT,
        MVAR_ERR_RATIONAL_EXPECTED,
        MVAR_ERR_WRONG_ORDER,
        MVAR_ERR_KNOT_NOT_ORDERED,
        MVAR_ERR_NUM_KNOT_MISMATCH,
        MVAR_ERR_INDEX_NOT_IN_MESH,
        MVAR_ERR_POWER_NO_SUPPORT,
        MVAR_ERR_WRONG_DOMAIN,
        MVAR_ERR_INCONS_DOMAIN,
        MVAR_ERR_SCALAR_PT_EXPECTED,
        MVAR_ERR_WRONG_PT_TYPE,
        MVAR_ERR_INVALID_AXIS,
        MVAR_ERR_NO_CLOSED_POLYGON,
        MVAR_ERR_TWO_INTERSECTIONS,
        MVAR_ERR_NO_MATCH_PAIR,
        MVAR_ERR_FAIL_READ_FILE,
        MVAR_ERR_INVALID_STROKE_TYPE,
        MVAR_ERR_READ_FAIL,
        MVAR_ERR_MVS_INCOMPATIBLE,
        MVAR_ERR_PT_OR_LEN_MISMATCH,
        MVAR_ERR_TOO_FEW_PARAMS,
        MVAR_ERR_TOO_MANY_PARAMS,
        MVAR_ERR_FAIL_CMPT,
        MVAR_ERR_NO_CROSS_PROD,
        MVAR_ERR_BEZIER_EXPECTED,
        MVAR_ERR_BSPLINE_EXPECTED,
        MVAR_ERR_BEZ_OR_BSP_EXPECTED,
        MVAR_ERR_SAME_GTYPE_EXPECTED,
        MVAR_ERR_SAME_PTYPE_EXPECTED,
        MVAR_ERR_ONE_OR_THREE_EXPECTED,
        MVAR_ERR_POWER_EXPECTED,
        MVAR_ERR_MSC_TOO_FEW_OBJ,
        MVAR_ERR_MSC_FAILED,
        MVAR_ERR_MSS_INCONSISTENT_NUM_OBJ,
        MVAR_ERR_SCALAR_EXPECTED,
        MVAR_ERR_DIM_TOO_HIGH,
        MVAR_ERR_INVALID_MV,
        MVAR_ERR_CANNT_MIX_BSP_BEZ,
        MVAR_ERR_CH_FAILED,
        MVAR_ERR_MSC_CURVES,
        MVAR_ERR_ROUND_CURVE,
        MVAR_ERR_ONLY_2D,
        MVAR_ERR_ONLY_3D,
        MVAR_ERR_2D_OR_3D,
        MVAR_ERR_1D_OR_3D,
        MVAR_ERR_WRONG_INDEX,
        MVAR_ERR_MSC_TOO_FEW_PTS,
        MVAR_ERR_ET_DFRNT_DOMAINS,
        MVAR_ERR_SRF_NOT_ADJ,
        MVAR_ERR_CURVATURE_CONT,
        MVAR_ERR_ZER_PRBLM_CNSTRCT,	
        MVAR_ERR_ZER_ORDER_CNSTRCT,
        MVAR_ERR_ZER_NUMERIC_TOO_EARLY,
        MVAR_ERR_ZER_SOL_CNSTRCT,
        MVAR_ERR_ZER_SCT_TOO_EARLY,
        MVAR_ERR_ZER_GZT_TOO_EARLY,
        MVAR_ERR_TWO_SAME_MVS,
        MVAR_ERR_TWO_SAME_INPUTS,
        MVAR_ERR_ZER_SINGULAR_SOLS,
        MVAR_ERR_ZER_CRT_PTS_NO_SUPPORT,
        MVAR_ERR_EXPR_TREE_NO_SUPPORT,
        MVAR_ERR_INVALID_INPUT,
        MVAR_ERR_INV_PROJ_FAILED,
        MVAR_ERR_UNDEFINE_ERR
    }
}