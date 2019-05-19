namespace IritNet
{
    public unsafe struct UserGCSolutionIndexStruct
    {
        public int ObsPtGroupIndex,       /* The indexes of the visibility map in the   */
             SuggestionIndex,       /* groups and suggestions.                    */
             Index;       /* The index of the visibility map when looking at all  */
             /* the visibility maps as one int list (the index in   */
             /* which it is saved to disk).                          */
    }
}
