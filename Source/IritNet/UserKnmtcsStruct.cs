namespace IritNet
{
    public unsafe struct UserKnmtcsStruct
    {
        public double XMin;
        public double XMax;
        public double YMin;
        public double YMax;
        public double ZMin;
        public double ZMax;
        public int PtsNum;
        public int BarsNum;
        public int ConstraintsNum;
        public  UserKnmtcsPtStruct *Pts;                   /* Pointer to point list. */
        public  UserKnmtcsBarStruct *Bars;                     /* Pointer to bar list. */
        public  UserKnmtcsConstrStruct *Constraints;             /* List of raints. */
    }
}
