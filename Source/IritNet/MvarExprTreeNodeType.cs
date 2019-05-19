namespace IritNet
{
    public enum MvarExprTreeNodeType
    {
        MVAR_ET_NODE_NONE,
        MVAR_ET_NODE_LEAF,
        MVAR_ET_NODE_ADD,
        MVAR_ET_NODE_SUB,
        MVAR_ET_NODE_MULT,
        MVAR_ET_NODE_SCALAR_MULT,
        MVAR_ET_NODE_MERGE,
        MVAR_ET_NODE_DOT_PROD,
        MVAR_ET_NODE_CROSS_PROD,
        MVAR_ET_NODE_EXP,
        MVAR_ET_NODE_LOG,
        MVAR_ET_NODE_COS,
        MVAR_ET_NODE_SQRT,
        MVAR_ET_NODE_SQR,
        MVAR_ET_NODE_NPOW,
        MVAR_ET_NODE_RECIP,
        MVAR_ET_NODE_COMMON_EXPR
    }
}