namespace IritNet
{
    public unsafe struct TrivIGAFieldStruct
    {
        public TrivIGAFieldStruct *Pnext;
        public TrivIGATVStruct *TVs;                         /* A list of trivariates. */
        public TrivIGAFieldType ValueType;
        public TrivIGAMaterialStruct *Material;
        public fixed byte NamedType[Irit.TRIV_IGA_MAX_FIELD_TYPE_LEN];
        public TrivIGAXMLPropertyArray16 Properties;
        public int NumProperties;
        public int ID;
    }
}