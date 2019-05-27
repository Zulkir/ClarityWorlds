using Clarity.Common.CodingUtilities.Collections;

namespace Clarity.Engine.EventRouting
{
    public abstract class RoutedEventBase : IRoutedEvent
    {
        public bool StopPropagation { get; set; }
        private IPropertyBag valueBag;

        public IPropertyBag ValueBag => 
            valueBag = valueBag ?? new PropertyBag();
    }
}