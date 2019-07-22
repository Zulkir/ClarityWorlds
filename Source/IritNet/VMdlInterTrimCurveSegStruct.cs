namespace IritNet
{
    public unsafe struct VMdlInterTrimCurveSegStruct
    {
        public  VMdlInterTrimCurveSegStruct* Pnext;
        public  IPAttributeStruct* Attr;
        /* List of trimmed surfaces sharing this boundary, Typically  */
        /* two surfaces.                                              */
        public  VMdlInterTrimSrfRefStruct* IncidentSrfsRefList;
        
        /* The actual trimmed curve. */
        /* Euclidean curve */
        public CagdCrvStruct* TrimSeg;
        /*MdlTrimSegStruct* TrimSeg; TODO - MdlTrimSegStruct or CagdCrvStruct */
        
        /* Start & End intersection points. If the end point is NULL, then */
        /* the curve seg. is closed.                                       */
        public VMdlInterTrimPointRefStruct* StartPntRef;
        public VMdlInterTrimPointRefStruct* EndPntRef;
    }
}
