using System;
using Clarity.Engine.Interaction.Input;

namespace Clarity.App.Worlds.Interaction.Tools
{
    public interface ITool : IDisposable
    {
        bool TryHandleInputEvent(IInputEventArgs eventArgs);
    }
}