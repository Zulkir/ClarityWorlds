namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtEmbeddingHandlerContainer
    {
        IRtEmbeddingHandler GetHandler(IRtEmbeddingSpan embedding);
    }
}