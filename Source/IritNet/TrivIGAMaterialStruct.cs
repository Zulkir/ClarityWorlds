namespace IritNet
{
    public unsafe struct TrivIGAMaterialStruct
    {
        public int Id;
        public fixed byte Name[Irit.TRIV_IGA_MAX_MATERIAL_NAME_LEN];
        public fixed byte Type[Irit.TRIV_IGA_MAX_MATERIAL_NAME_LEN];
        public TrivIGAXMLPropertyArray16 Properties;
        public int NumProperties;
    }
}