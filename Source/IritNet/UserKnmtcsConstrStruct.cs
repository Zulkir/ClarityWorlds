namespace IritNet
{
    public unsafe struct UserKnmtcsConstrStruct
    {
        public struct UnionV
        {
            public double distance;
            public double angle {  get { return distance; } set { distance = value; } }
        }

        public struct UnionC
        {
            public struct DstPtPtStruct
            {
                public UserKnmtcsPtStruct* Pt1;              /* Distance PT_PT. */
                public UserKnmtcsPtStruct* Pt2;
            }
            
            public struct DstPtBarStruct
            {
                public UserKnmtcsPtStruct* Pt;          /* Distance PT_BAR. */
                public UserKnmtcsBarStruct* Bar;
            }
            
            public struct DstPtCrvStruct
            {
                public UserKnmtcsPtStruct* Pt;          /* Distance PT_CRV. */
                public UserKnmtcsPtStruct* CrvPt;                /* Foot point. */
            }

            public struct DstPtSrfStruct
            {
                public UserKnmtcsPtStruct* Pt;          /* Distance PT_SRF. */
                public UserKnmtcsPtStruct* SrfPt;			      /* Foot point. */
            }

            public struct DstBarBarStruct
            {
                public UserKnmtcsBarStruct* Bar1;          /* Distance BAR_BAR. */
                public UserKnmtcsBarStruct* Bar2;
            }

            public struct DstBarCrvStruct
            {
                public UserKnmtcsBarStruct* Bar;           /* Distance BAR_CRV. */
                public CagdCrvStruct* Crv;
            }

            public struct AngleStruct
            {
                public UserKnmtcsBarStruct* Bar1;          /* Angle, orthogonality. */
                public UserKnmtcsBarStruct* Bar2;
            }

            public struct TanStruct
            {
                public UserKnmtcsBarStruct* Bar;               /* Tangnecy. */
                public UserKnmtcsPtStruct* Pt;			   /* Contact point. */
            }

            public struct TanSrfStruct
            {
                public UserKnmtcsBarStruct* Bar;               /* Tangnecy. */
                public CagdSrfStruct* Srf;
                public UserKnmtcsPtStruct* Pt;			   /* Contact point. */
            }

            public struct ParStruct
            {
                public UserKnmtcsBarStruct* Bar1;                  /* Parallel. */
                public UserKnmtcsBarStruct* Bar2;
            }

            public DstPtPtStruct DstPtPt {  get { var loc = TanSrf; return *(DstPtPtStruct*)&loc; } set { var loc = this; *(DstPtPtStruct*)&loc = value; this = loc; } }
            public DstPtBarStruct DstPtBar { get { var loc = TanSrf; return *(DstPtBarStruct*)&loc; } set { var loc = this; *(DstPtBarStruct*)&loc = value; this = loc; } }
            public DstPtCrvStruct DstPtCrv { get { var loc = TanSrf; return *(DstPtCrvStruct*)&loc; } set { var loc = this; *(DstPtCrvStruct*)&loc = value; this = loc; } }
            public DstPtSrfStruct DstPtSrf { get { var loc = TanSrf; return *(DstPtSrfStruct*)&loc; } set { var loc = this; *(DstPtSrfStruct*)&loc = value; this = loc; } }
            public DstBarBarStruct DstBarBar { get { var loc = TanSrf; return *(DstBarBarStruct*)&loc; } set { var loc = this; *(DstBarBarStruct*)&loc = value; this = loc; } }
            public DstBarCrvStruct DstBarCrv { get { var loc = TanSrf; return *(DstBarCrvStruct*)&loc; } set { var loc = this; *(DstBarCrvStruct*)&loc = value; this = loc; } }
            public AngleStruct Angle { get { var loc = TanSrf; return *(AngleStruct*)&loc; } set { var loc = this; *(AngleStruct*)&loc = value; this = loc; } }
            public TanStruct Tan { get { var loc = TanSrf; return *(TanStruct*)&loc; } set { var loc = this; *(TanStruct*)&loc = value; this = loc; } }
            public TanSrfStruct TanSrf;
            public ParStruct Par { get { var loc = TanSrf; return *(ParStruct*)&loc; } set { var loc = this; *(ParStruct*)&loc = value; this = loc; } }
        }

        public UserKnmtcsConstrStruct *Pnext;
        public UserKnmtcsConstrType Type;
        public UnionV V;
        public UnionC C;
    }
}