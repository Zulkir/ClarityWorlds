namespace IritNet
{
    public unsafe struct UserMicroParamStruct
    {
        public struct Union
        {
            public UserMicroRegularParamStruct RegularParam; // 4p + 7d
            public UserMicroRandomParamStruct RandomParam { get { var loc = RegularParam; return *(UserMicroRandomParamStruct*)&loc; } set { var loc = this; *(UserMicroRandomParamStruct*)&loc = value; this = loc; } } // 7d
            public UserMicroFunctionalParamStruct FunctionalParam { get { var loc = RegularParam; return *(UserMicroFunctionalParamStruct*)&loc; } set { var loc = this; *(UserMicroFunctionalParamStruct*)&loc = value; this = loc; } } // 7d + 1p
        }

        public MvarMVStruct* DeformMV;         /* The freeform mdefromatiuon function. */
        public int ShellCapBits;  /* 6 bits controlling global boundary shelling and 6 */
        /* bits for global capping alternatives. See USER_MICRO_BIT_SHELL/CAP_*.*/
        public double ShellThickness;  /* The thickness of a SHELL global boudnary. */
        public UserMicroTilingType TilingType;/* Type of tiling - controls union below.*/
        public Union U;
    }
}