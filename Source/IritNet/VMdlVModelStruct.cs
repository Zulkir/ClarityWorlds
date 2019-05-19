namespace IritNet
{
    public unsafe struct VMdlVModelStruct
    {
        public  VMdlVModelStruct *Pnext;
        public  IPAttributeStruct *Attr;
             /* List of volumetric elements in the model. */
        public VMdlVolumeElementStruct *VolumeElements;
             /* All surfaces in the model in the model. */
        public VMdlInterTrimSrfStruct *InterSrfList;
             /* All trimming curve segments in the model. */
        public VMdlInterTrimCurveSegStruct *InterCurveSegList;
             /* All trimming curves end points in the model. */
        public VMdlInterTrimPointStruct *InterPointList;
             /* All TV's in the entire model in the model. */
        public TrivTVStruct *TVList;
    }
}
