namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public class TrwSerializationOptions
    {
        public TrwSerializationExplicitTypes ExplicitTypes { get; set; }
        public bool AliasTypes { get; set; }
        public string TypePropertyName { get; set; }
        public string ValuePropertyName { get; set; }
    }
}