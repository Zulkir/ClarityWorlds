using System;
using Clarity.Engine.Interaction.Input;

namespace Clarity.Core.AppCore.Tools
{
    public interface ITool : IDisposable
    {
        bool TryHandleInputEvent(IInputEventArgs eventArgs);
    }
}