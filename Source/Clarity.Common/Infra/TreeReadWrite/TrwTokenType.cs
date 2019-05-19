namespace Clarity.Common.Infra.TreeReadWrite
{
    public enum TrwTokenType
    {
        None,
        StartObject,
        EndObject,
        StartArray,
        EndArray,
        PropertyName,
        Null,
        Boolean,
        Integer,
        Float,
        String,
    }
}