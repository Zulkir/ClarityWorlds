using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Engine.Media.Text.Rich
{
    public class RtEmbeddingHandlerContainer : IRtEmbeddingHandlerContainer
    {
        private readonly IReadOnlyList<IRtEmbeddingHandler> handlers;
        private readonly Dictionary<string, IRtEmbeddingHandler> handlersByType;

        public RtEmbeddingHandlerContainer(IReadOnlyList<IRtEmbeddingHandler> handlers)
        {
            this.handlers = handlers;
            handlersByType = new Dictionary<string, IRtEmbeddingHandler>();
        }

        public IRtEmbeddingHandler GetHandler(IRtEmbeddingSpan embedding)
        {
            // todo: handle the case when no handler found
            return handlersByType.GetOrAdd(embedding.EmbeddingType, x => handlers.Last(h => h.HandledTypes.Contains(x)));
        }
    }
}