namespace IritNet
{
    public unsafe struct UserGCObsPtGroupTypeStruct
    {
        public UserGCObsPtTypeStruct ObsPtType;
        public UserGCPreDefinedObsPtSuggestionsStruct PredefinedSuggestions;
        public UserGCObsPtSuggestionStruct **Suggestions;
    }
}
