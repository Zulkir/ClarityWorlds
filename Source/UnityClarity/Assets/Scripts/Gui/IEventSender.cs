using System;

namespace Assets.Scripts.Gui
{
    public interface IEventSender
    {
        event Action OnGUIEvent;
        event Action UpdateEvent;
    }
}