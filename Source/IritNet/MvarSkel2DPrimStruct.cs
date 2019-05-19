namespace IritNet
{
    public unsafe struct MvarSkel2DPrimStruct
    {
        public MvarSkel2DPrimStruct *Pnext;
        public IPAttributeStruct *Attr;
        public MvarSkel2DPrimType Type;
        public MvarSkel2DPrimPointStruct Pt { get { var loc = Ln; return *(MvarSkel2DPrimPointStruct*)&loc; } set { var loc = Ln; *(MvarSkel2DPrimPointStruct*)&loc = value; Ln = loc; } }
        public MvarSkel2DPrimLineStruct Ln;
        public MvarSkel2DPrimArcStruct Arc { get { var loc = Ln; return *(MvarSkel2DPrimArcStruct*)&loc; } set { var loc = Ln; *(MvarSkel2DPrimArcStruct*)&loc = value; Ln = loc; } }
        public MvarSkel2DPrimCrvStruct Crv { get { var loc = Ln; return *(MvarSkel2DPrimCrvStruct*)&loc; } set { var loc = Ln; *(MvarSkel2DPrimCrvStruct*)&loc = value; Ln = loc; } }
        public int _Index;
        public CagdCrvStruct* _CrvRep;
    }
}