using Clarity.Common.Numericals.Colors;
using ObjectGL.Api.Context;
using ObjectGL.Api.Context.Actions;

namespace Clarity.Ext.Rendering.Ogl3
{
    public class DrawElementsCall : IDrawCall
    {
        private readonly BeginMode mode;
        private readonly int indexCount;
        private readonly DrawElementsType indexType;
        private readonly int indexBufferOffset;
        public Color4? OverwriteColor { get; }

        public DrawElementsCall(BeginMode mode, int indexCount, DrawElementsType indexType, int indexBufferOffset, Color4? overwriteColor)
        {
            this.mode = mode;
            this.indexCount = indexCount;
            this.indexType = indexType;
            this.indexBufferOffset = indexBufferOffset;
            OverwriteColor = overwriteColor;
        }

        public void Issue(IContext glContext)
        {
            glContext.Actions.Draw.Elements(mode, indexCount, indexType, indexBufferOffset);
        }
    }
}