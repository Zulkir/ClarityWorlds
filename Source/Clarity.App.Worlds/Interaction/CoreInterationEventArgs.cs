using Clarity.Engine.Visualization.Viewports;

namespace Clarity.App.Worlds.Interaction
{
    public class CoreInterationEventArgs : ICoreInterationEventArgs
    {
        public CoreInteractionEventCategory Category { get; set; }
        public CoreInteractionEventType Type { get; set; }
        public IViewport Viewport { get; set; }
        
        public static CoreInterationEventArgs Selected() => 
            new CoreInterationEventArgs
            {
                Category = CoreInteractionEventCategory.PrimarySelection,
                Type = CoreInteractionEventType.Happened
            };

        public static CoreInterationEventArgs Deselected() => 
            new CoreInterationEventArgs
            {
                Category = CoreInteractionEventCategory.PrimarySelection,
                Type = CoreInteractionEventType.Released
            };
    }
}