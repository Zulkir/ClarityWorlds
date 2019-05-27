using Clarity.Common.Infra.ActiveModel;
using Clarity.Engine.EventRouting;

namespace Clarity.App.Worlds.WorldTree
{
    public interface IWorldTreeUpdatedEvent : IRoutedEvent
    {
        IAmEventMessage AmMessage { get; }
    }
}