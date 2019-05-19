namespace IritNet
{
    public unsafe struct TrivIGAXMLProperty
    {
        public fixed byte Name[Irit.TRIV_IGA_MAX_MATERIAL_NAME_LEN];
        public fixed byte Value[Irit.TRIV_IGA_MAX_MATERIAL_NAME_LEN];
    }
}