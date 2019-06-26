using Clarity.Engine.EventRouting;
using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction
{
    public class CoreInterationEvent : RoutedEventBase, ICoreInterationEvent
    {
        public CoreInteractionEventCategory Category { get; set; }
        public CoreInteractionEventType Type { get; set; }
        public IViewport Viewport { get; set; }
        
        public static CoreInterationEvent Selected() => 
            new CoreInterationEvent
            {
                Category = CoreInteractionEventCategory.PrimarySelection,
                Type = CoreInteractionEventType.Happened
            };

        public static CoreInterationEvent Deselected() => 
            new CoreInterationEvent
            {
                Category = CoreInteractionEventCategory.PrimarySelection,
                Type = CoreInteractionEventType.Released
            };
    }
}