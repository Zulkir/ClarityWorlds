namespace IritNet
{
    // size: 1p + 2i + 3d + 1p + 3d + 1p = 3p + 2i + 6d = 3p + 7d
    public unsafe struct UserKnmtcsPtStruct
    {
        public struct Union
        {
            public CagdCrvStruct* Crv;			    /* Pt moves along curve. */
            public CagdSrfStruct* Srf { get { return (CagdSrfStruct*)Crv; } set { Crv = (CagdCrvStruct*)value; } }		 	  /* Pt moves along surface. */
        }

        public UserKnmtcsPtStruct *Pnext;
        public UserKnmtcsMovabilityPointType Type;
        public int Idx;
        public IrtPtType Pt;
        public Union U;
        public CagdPtStruct Center;                      /* If Crv rotates. */ // 3d + 2p
    }
}