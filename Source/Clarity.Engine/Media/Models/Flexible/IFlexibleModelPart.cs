namespace Clarity.Engine.Media.Models.Flexible
{
    public interface IFlexibleModelPart
    {
        string ModelMaterialName { get; }

        // todo: Transform, TransformSpace

        int VertexSetIndex { get; }
        FlexibleModelPrimitiveTopology PrimitiveTopology { get; }
        int IndexCount { get; }
        int FirstIndex { get; }
        int VertexOffset { get; }
    }
}