using Clarity.Common.CodingUtilities.Collections;

namespace Clarity.Engine.EventRouting
{
    public interface IRoutedEvent
    {
        bool StopPropagation { get; set; }
        IPropertyBag ValueBag { get; }
    }
}