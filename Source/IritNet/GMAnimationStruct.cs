namespace IritNet
{
    public unsafe struct GMAnimationStruct
    {
        public double
            StartT,		                     /* Starting time of animation. */
            FinalT,		                  /* Termination time of animation. */
            Dt,		                         /* Step size pf animation. */
            RunTime;		              /* Current time of animation. */
        public int TwoWaysAnimation,   /* Should the animation bounce back and forth!? */
            SaveAnimationGeom,          /* Save animation geometry into files!? */
            SaveAnimationImage,           /* Save animation images into files!? */
            BackToOrigin,	           /* Should we terminate at the beginning? */
            NumOfRepeat,			            /* How many iterations? */
            StopAnim,		   /* If TRUE, must stop the animation now. */
            SingleStep,			     /* Are we in single step mode? */
            TextInterface,		/* Are we communicating using a textual UI? */
            MiliSecSleep,	  /* How many milliseconds to sleep between frames. */
            _Count;						/* Used internally. */
        public byte
            *ExecEachStep;		      /* Program to execute each iteration. */
        public fixed byte
            BaseFileName[Irit.IRIT_LINE_LEN]; /* Base name of animation files saved. */
    }
}