using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Worlds.WorldTree
{
    public class WorldTreeUpdatedEvent : RoutedEventBase, IWorldTreeUpdatedEvent
    {
        public IAmEventMessage AmMessage { get; }

        public WorldTreeUpdatedEvent(IAmEventMessage amMessage)
        {
            AmMessage = amMessage;
        }
    }
}