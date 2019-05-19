namespace IritNet
{
    public unsafe struct MvarExprTreeStruct
    {
        public struct Union
        {
            public struct Struct1
            {
                public MvarMVStruct *MV;
                public int IsRef;       /* TRUE if an MV reference - do not free. */
            }

            public struct Struct2
            {
                public MvarExprTreeStruct *Left, Right;
            }

            public Struct1 S1 { get { var loc = this; return *(Struct1*)&loc; } set { var loc = this; *(Struct1*)&loc = value; this = loc; } }
            public Struct2 S2;
        }

        public MvarExprTreeNodeType NodeType;
        public int Dim;
        public int PtSize;					    /* vector function size. */
        public Union _U;
        public MvarMVStruct *MV { get { return _U.S1.MV; } set { var u = _U; var s1 = u.S1; s1.MV = value; u.S1 = s1; _U = u; } }
        public int IsRef { get { return _U.S1.IsRef; } set { var u = _U; var s1 = u.S1; s1.IsRef = value; u.S1 = s1; _U = u; } }
        public MvarExprTreeStruct *Left { get { return _U.S2.Left; } set { var u = _U; var s2 = u.S2; s2.Left = value; u.S2 = s2; _U = u; } }
        public MvarExprTreeStruct *Right { get { return _U.S2.Right; } set { var u = _U; var s2 = u.S2; s2.Right = value; u.S2 = s2; _U = u; } }
        public MvarNormalConeStruct *MVBCone;
        public GMBBBboxStruct MVBBox;
        public int Val;			             /* Integer value for constants. */
        public int IAux, IAux2;		     /* Auxiliary integers for internal use. */
        public void* PAux;		      /* Auxiliary pointer for internal use. */
        public byte *Info;			   /* Optional info on this expression tree. */
    }
}