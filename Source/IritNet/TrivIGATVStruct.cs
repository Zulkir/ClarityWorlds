namespace IritNet
{
    public unsafe struct TrivIGATVStruct
    {
        public TrivIGATVStruct *Pnext;
        public TrivTVStruct *TV;
        public TrivTVStruct* DuTV, DvTV, DwTV,              /* Derivative TVs to TV. */
        Du2TV, Dv2TV, Dw2TV, DuDvTV, DuDwTV, DvDwTV;

        public TrivIGAAdjacencyInfoStructArray6 Neighbors;
        public int UniqueCtlPtIDMin, UniqueCtlPtIDMax;
        public TrivIGACtlPtUniqueIDsStruct *CtlPtsIDs;
        public TrivIGAFieldStruct *Field;/* Back reference to the parent field. */
    }
}