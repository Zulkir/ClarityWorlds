namespace IritNet
{
    public unsafe struct VMdlInterTrimSrfStruct
    {
        public  VMdlInterTrimSrfStruct *Pnext;
        public  IPAttributeStruct *Attr;
             /* The volumetric element this is one of its faces. */
        public  VMdlVolumeElementRefStruct *TrimmedTVRef;
             /* The adjacent Srf to this one.  Can be NULL if none. */
        public  VMdlInterTrimSrfRefStruct *OppositeSrfRef;
             /* List of trimming loops. */
        public VMdlInterTrimCurveSegLoopInSrfStruct **BoundaryTrimmingCurveLoops;
        public  int NumOfBoundaryLoops;
             /* The real surface geometry. */
        public CagdSrfStruct *UVWSrf;
        public VMdlInterBndryType BndryType;
    }
}
