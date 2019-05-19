using Clarity.Common.Infra.ActiveModel;

namespace Clarity.Engine.Media.Text.Rich
{
    public interface IRtParagraphStyle : IAmObject
    {
        RtParagraphAlignment Alignment { get; set; }
        RtParagraphDirection Direction { get; set; }
        RtListType ListType { get; set; }
        IRtListStyle ListStyle { get; }
        int TabCount { get; set; }
        float MarginUp { get; set; }
    }
}