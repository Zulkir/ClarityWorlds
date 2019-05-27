namespace Clarity.Common.Infra.TreeReadWrite.Paths 
{
    public class TrwPathPartProperty : TrwPath
    {
        public string PropertyName { get; }

        public override string ToString() => $"{BasePath}.{PropertyName}";

        public TrwPathPartProperty(TrwPath basePath, string propertyName) 
            : base(basePath)
        {
            PropertyName = propertyName;
        }
    }
}