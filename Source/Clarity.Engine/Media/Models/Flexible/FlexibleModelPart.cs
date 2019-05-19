namespace Clarity.Engine.Media.Models.Flexible
{
    public class FlexibleModelPart : IFlexibleModelPart
    {
        public string ModelMaterialName { get; set; }

        public int VertexSetIndex { get; set; }
        public FlexibleModelPrimitiveTopology PrimitiveTopology { get; set; }
        public int IndexCount { get; set; }
        public int FirstIndex { get; set; }
        public int VertexOffset { get; set; }
    }
}