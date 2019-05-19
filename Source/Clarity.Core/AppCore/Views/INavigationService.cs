using System;
using System.Collections.Generic;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.Views
{
    public interface INavigationService
    {
        ISceneNode Current { get; }
        ISceneNode Previous { get; }
        NavigationState State { get; }
        bool InterLevelTransition { get; }
        IReadOnlyList<string> ForkOptions { get; }

        void GoToSpecific(int id);
        void GoForOption(int? option);

        void Reset(int id);

        bool TryHandleInput(IInputEventArgs args);

        void OnFocus(int newFocusId);

        event Action<INavigationEventArgs> Updated;
    }
}