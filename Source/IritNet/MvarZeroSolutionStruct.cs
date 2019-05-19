namespace IritNet
{
    public unsafe struct MvarZeroSolutionStruct
    {
        public struct Union
        {
            public MvarPtStruct* Pt;
            public MvarPolylineStruct* Pl { get { return (MvarPolylineStruct*)Pt; } set { Pt = (MvarPtStruct*)value; } }
            public MvarTriangleStruct* Tr { get { return (MvarTriangleStruct*)Pt; } set { Pt = (MvarPtStruct*)value; } }
        }

        public MvarZeroSolutionStruct *Pnext;
        public IPAttributeStruct *Attr;
        public Union U;
        public MvarZrSlvrRepresentationType ActiveRepresentation;
        public MvarZeroTJunctionStruct * TJList;/*of current problem, not handled.*/
}
}