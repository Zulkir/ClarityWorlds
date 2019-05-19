namespace IritNet
{
    public unsafe struct IPNCGCodeLineStruct
    {
        public int StreamLineNumber;                       /* Stream/File line number. */
        public int GCodeLineNumber;        /* G code's Nxxxx if has one, -1 otherwise. */
        public byte *Line;                        /* A copy of the H code as a string. */
        public IPNCGCodeLineType GCodeType;
        public fixed double XYZ[3], IJK[3];                /* Cutter position/orientation. */
        public double FeedRate, UpdatedFeedRate;
        public double SpindleSpeed;
        public double LenStart, Len;       /* Length from start and from last point. */
        public double EFactor;     /* Used in AM to set flow of material deposition. */
        public int ToolNumber;
        public int IsVerticalUpMotion;		  /* TRUE if we move up vertically. */
        public int Comment;
        public int HasMotion;		 /* TRUE if this line performs some motion. */
        public CagdCrvStruct *Crv;
    }
}