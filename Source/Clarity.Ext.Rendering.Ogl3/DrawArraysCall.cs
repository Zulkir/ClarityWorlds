using Clarity.Common.Numericals.Colors;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class DrawArraysCall : IDrawCall
    {
        private readonly BeginMode mode;
        private readonly int firstVertex;
        private readonly int vertexCount;
        public Color4? OverwriteColor => null;

        public DrawArraysCall(BeginMode mode, int firstVertex, int vertexCount)
        {
            this.mode = mode;
            this.firstVertex = firstVertex;
            this.vertexCount = vertexCount;
        }

        public void Issue(IContext glContext)
        {
            glContext.Actions.Draw.Arrays(mode, firstVertex, vertexCount);
        }
    }
}