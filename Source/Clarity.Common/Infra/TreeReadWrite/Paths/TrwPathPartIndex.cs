namespace Clarity.Common.Infra.TreeReadWrite.Paths 
{
    public class TrwPathPartIndex : TrwPath
    {
        public int Index { get; }

        public override string ToString() => $"{BasePath}[{Index}]";

        public TrwPathPartIndex(TrwPath basePath, int index)
            : base(basePath)
        {
            Index = index;
        }
    }
}