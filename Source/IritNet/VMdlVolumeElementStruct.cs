namespace IritNet
{
    public unsafe struct VMdlVolumeElementStruct
    {
        public  VMdlVolumeElementStruct *Pnext;
        public  IPAttributeStruct *Attr;
             /* Boundary surfaces. */
        public VMdlInterTrimSrfRefStruct *BoundarySrfRefList;
             /* Associated trimmed curve segments. Maybe redundancy !? */
        public VMdlInterTrimCurveSegRefStruct *TrimCurveSegRefList;
             /* All trimming curves' end points. Maybe redundancy !? */
        public VMdlInterTrimPointRefStruct *TrimPointRefList;
             /* All TVs that their intersection created this element. */
        public VMdlInterTrivTVRefStruct  *TVRefList;
             /* A 2D model that represents the boundaries of this element. */
        public MdlModelStruct *__BoundaryModel;
    }
}
