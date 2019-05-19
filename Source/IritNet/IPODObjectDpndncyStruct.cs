namespace IritNet
{
    public unsafe struct IPODObjectDpndncyStruct
    {
        public IPODObjectDpndncyStruct* Pnext;	        /* To next in chain. */
        public IPAttributeStruct *Attr;
        public IPODParamsStruct* ObjParams;  /* Objects that are params to this. */
        public IPODDependsStruct* ObjDepends;/* Objects who depends on this obj. */
        public byte* EvalExpr;          /* An assingment string that updates an object. */
        public int EvalIndex;    /* A simple measure against circular dependencies. */
        public int NumVisits;  /* Number of times node is traversed in graph traversal. */
        public int NumParams;       /* Number of parameters (length of ObjParams list). */
    }
}