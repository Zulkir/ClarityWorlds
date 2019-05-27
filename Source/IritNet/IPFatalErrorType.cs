﻿namespace IritNet
{
    public enum IPFatalErrorType
    {
        IP_ERR_NO_LINE_NUM = -100, /* Signals no line num of error is avialable. */

        IP_ERR_NONE = 0,

        IP_ERR_ALLOC_FREED_LOOP,
        IP_ERR_PT_OBJ_EXPECTED,
        IP_ERR_LIST_OBJ_EXPECTED,
        IP_ERR_LIST_OBJ_SHORT,
        IP_ERR_DEL_OBJ_NOT_FOUND,
        IP_ERR_LOCASE_OBJNAME,
        IP_ERR_UNDEF_ATTR,
        IP_ERR_PTR_ATTR_COPY,
        IP_ERR_UNSUPPORT_CRV_TYPE,
        IP_ERR_UNSUPPORT_SRF_TYPE,
        IP_ERR_UNSUPPORT_TV_TYPE,
        IP_ERR_UNSUPPORT_TRNG_TYPE,
        IP_ERR_NOT_SUPPORT_CNVRT_IRT,
        IP_ERR_NEIGH_SEARCH,
        IP_ERR_VRTX_HASH_FAILED,
        IP_ERR_INVALID_STREAM_HNDL,
        IP_ERR_STREAM_TBL_FULL,
        IP_ERR_LIST_CONTAIN_SELF,
        IP_ERR_UNDEF_OBJECT_FOUND,
        IP_ERR_ILLEGAL_FLOAT_FRMT,
        IP_ERR_NON_LIST_IGNORED,
        IP_ERR_LIST_TOO_LARGE,
        IP_ERR_LESS_THAN_3_VRTCS,
        IP_ERR_FORK_FAILED,
        IP_ERR_CLOSED_SOCKET,
        IP_ERR_READ_LINE_TOO_LONG,
        IP_ERR_NUMBER_EXPECTED,
        IP_ERR_OPEN_PAREN_EXPECTED,
        IP_ERR_CLOSE_PAREN_EXPECTED,
        IP_ERR_LIST_COMP_UNDEF,
        IP_ERR_UNDEF_EXPR_HEADER,
        IP_ERR_PT_TYPE_EXPECTED,
        IP_ERR_OBJECT_EMPTY,
        IP_ERR_FILE_EMPTY,
        IP_ERR_FILE_NOT_FOUND,
        IP_ERR_MIXED_TYPES,
        IP_ERR_STR_NOT_IN_QUOTES,
        IP_ERR_STR_TOO_LONG,
        IP_ERR_OBJECT_EXPECTED,
        IP_ERR_STACK_OVERFLOW,
        IP_ERR_DEGEN_POLYGON,
        IP_ERR_DEGEN_NORMAL,
        IP_ERR_SOCKET_BROKEN,
        IP_ERR_SOCKET_TIME_OUT,

        IP_ERR_CAGD_LIB_ERR,
        IP_ERR_TRIM_LIB_ERR,
        IP_ERR_TRIV_LIB_ERR,
        IP_ERR_TRNG_LIB_ERR,
        IP_ERR_MDL_LIB_ERR,
        IP_ERR_MVAR_LIB_ERR,

        IP_ERR_BIN_IN_TEXT,
        IP_ERR_BIN_UNDEF_OBJ,
        IP_ERR_BIN_WRONG_SIZE,
        IP_ERR_BIN_SYNC_FAIL,
        IP_ERR_BIN_PL_SYNC_FAIL,
        IP_ERR_BIN_CRV_SYNC_FAIL,
        IP_ERR_BIN_CRV_LIST_EMPTY,
        IP_ERR_BIN_SRF_SYNC_FAIL,
        IP_ERR_BIN_TSRF_SYNC_FAIL,
        IP_ERR_BIN_TCRV_SYNC_FAIL,
        IP_ERR_BIN_TV_SYNC_FAIL,
        IP_ERR_BIN_MV_SYNC_FAIL,
        IP_ERR_BIN_TRISRF_SYNC_FAIL,
        IP_ERR_BIN_MAT_SYNC_FAIL,
        IP_ERR_BIN_INST_SYNC_FAIL,
        IP_ERR_BIN_STR_SYNC_FAIL,
        IP_ERR_BIN_OLST_SYNC_FAIL,
        IP_ERR_BIN_ATTR_SYNC_FAIL,

        IP_ERR_NC_ARC_INVALID_RAD,
        IP_ERR_NC_MAX_ZBUF_SIZE_EXCEED,

        IP_ERR_ONLY_FREEFORM,
        IP_ERR_ONLY_CRV_SRF_MV,
        IP_ERR_ONLY_TRI_SRF,
        IP_ERR_ONLY_TRIM_SRF,
        IP_ERR_CNVRT_TO_PERIODIC,
        IP_ERR_CNVRT_PER_TO_FLOAT,
        IP_ERR_CNVRT_BSP_TO_FLOAT,
        IP_ERR_CNVRT_MV_NOT_UNIVAR,
        IP_ERR_CNVRT_MV_NOT_BIVAR,
        IP_ERR_CNVRT_MV_NOT_TRIVAR,
        IP_ERR_CNVRT_TSRF_TO_MDL,
        IP_ERR_CNVRT_SRF_MDL_TO_TSRF,
        IP_ERR_CNVRT_INVALID_GEOM_TO_MV,
        IP_ERR_CNVRT_INVALID_COERCE,

        IP_WRN_OBJ_NAME_TRUNC = 1000,

        IP_ERR_INFO_SHIFT = 10000
    }
}