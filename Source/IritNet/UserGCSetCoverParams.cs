namespace IritNet
{
    public unsafe struct UserGCSetCoverParams
    {
        public double CoverLimit;    /* Used only for exhaustive algorithms.          */
        public int SizeLimit;          /* Used only by exhaustive and exact algorithms. */
        public UserGCSetCoverAlgorithmType Algorithm;
    }
}
