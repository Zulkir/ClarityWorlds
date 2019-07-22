namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtEmbeddingSpan : IRtSpan
    {
        string EmbeddingType { get; }
        string SourceCode { get; set; }
    }
}