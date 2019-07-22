using System.Collections.Generic;
using Clarity.Engine.Interaction.Input.Mouse;
using Clarity.Engine.Media.Images;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtEmbeddingHandler
    {
        IReadOnlyList<string> HandledTypes { get; }
        IImage BuildImage(IRtEmbeddingSpan embedding); 
        bool TryHandleMouseEvent(IRtEmbeddingSpan embedding, IMouseEvent args);
    }
}