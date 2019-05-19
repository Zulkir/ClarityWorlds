namespace IritNet
{
    public enum IPObjStructType
    {
        IP_OBJ_ERROR = -1,
        IP_OBJ_UNDEF = 0,

        IP_OBJ_POLY,                     /* These are the objects in overload.c. */
        IP_OBJ_NUMERIC,
        IP_OBJ_POINT,
        IP_OBJ_VECTOR,
        IP_OBJ_PLANE,
        IP_OBJ_MATRIX,
        IP_OBJ_CURVE,
        IP_OBJ_SURFACE,
        IP_OBJ_STRING,
        IP_OBJ_LIST_OBJ,
        IP_OBJ_CTLPT,
        IP_OBJ_TRIMSRF,
        IP_OBJ_TRIVAR,
        IP_OBJ_INSTANCE,
        IP_OBJ_TRISRF,
        IP_OBJ_MODEL,
        IP_OBJ_MULTIVAR,
        IP_OBJ_VMODEL,

        IP_OBJ_ANY = 100         /* Match any object type, in type checking. */
    }
}