namespace IritNet
{
    // size: 3p
    public unsafe struct UserKnmtcsBarStruct
    {
        public UserKnmtcsBarStruct *Pnext;
        public UserKnmtcsPtStruct* P1, P2;      /* Starting & ending point of the bar. */
    }
}