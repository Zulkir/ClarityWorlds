namespace IritNet
{
    public unsafe struct VMdlInterTrimCurveSegLoopInSrfStruct
    {
             /* Next curve segment in loop. */
        public  VMdlInterTrimCurveSegLoopInSrfStruct *Pnext;
        public  IPAttributeStruct *Attr;
             /* The reference curve. */
        public  VMdlInterTrimCurveSegRefStruct *CrvRef;
             /* Next curve segment in loop. */
             /*  VMdlInterTrimCurveSegStruct *PnextCrv; */
             /* Previous curve segment in loop. */
        public  VMdlInterTrimCurveSegLoopInSrfStruct *PprevCrv;
             /* Surface holding this trimming loop. */
        public  VMdlInterTrimSrfRefStruct *IncidentSrfRef;
             
             /* The trimming curve in UV space of this Incident Srf. */
        public CagdCrvStruct *UVCrv;
        public int IsCrvInverted;
    }
}
