namespace IritNet
{
    public unsafe struct IritConfigStruct
    {
        public  byte *VarName;
        public  byte *SomeInfo;
        public void * VarData;
        public IrtCfgDataType VarType;
    }
}
