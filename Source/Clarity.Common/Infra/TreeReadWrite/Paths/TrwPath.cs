namespace Clarity.Common.Infra.TreeReadWrite.Paths
{
    public class TrwPath
    {
        public TrwPath BasePath { get; }

        public TrwPath(TrwPath basePath)
        {
            BasePath = basePath;
        }

        public TrwPath PushProperty(string propertyName) => new TrwPathPartProperty(this, propertyName);
        public TrwPath PushIndex(int index) => new TrwPathPartIndex(this, index);
        public TrwPath Pop() => BasePath;
    }
}