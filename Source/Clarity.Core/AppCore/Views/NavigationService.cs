using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Core.AppCore.StoryGraph;
using Clarity.Engine.Interaction.Input;
using Clarity.Engine.Interaction.Input.Keyboard;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Core.AppCore.Views
{
    public class NavigationService : INavigationService
    {
        private readonly IStoryService storyService;
        private readonly Stack<int> previousStack;
        private readonly Stack<int> nextStack;
        private int current;
        
        public NavigationState State { get; private set; }
        public bool InterLevelTransition => sg.Children[current].Any() || sg.Children[PreviousId].Any();
        public IReadOnlyList<string> ForkOptions { get; private set; }
        private int[] optionIds;

        private IStoryGraph sg;
        
        public event Action<INavigationEventArgs> Updated;

        private bool AtFork =>
            State == NavigationState.AtBackwardFork ||
            State == NavigationState.AtForwardFork;

        public NavigationService(IStoryService storyService)
        {
            current = -1;
            this.storyService = storyService;
            previousStack = new Stack<int>();
            nextStack = new Stack<int>();
            storyService.GraphChanged += OnStoryGraphChanged;
        }

        public ISceneNode Current => sg.NodeObjects[current];
        private int PreviousId => previousStack.Count > 0 ? previousStack.Peek() : current;
        public ISceneNode Previous => sg.NodeObjects[PreviousId];

        public void OnFocus(int newFocusId)
        {
            GoToSpecificInternal(newFocusId, true);
        }

        private void OnStoryGraphChanged()
        {
            sg = storyService.GlobalGraph;
            Reset(sg.NodeIds.Contains(current) 
                ? current 
                : sg.Root);
        }

        public bool CanGoNext => sg.Next[current].Any();

        public void GoNext()
        {
            var candidateList = sg.Next[current];
            if (candidateList.Count == 0)
                throw new InvalidOperationException();

            previousStack.Push(current);
            if (candidateList.Count == 1)
            {
                SetFork(NavigationState.AtNode, null);
                current = candidateList.Single();
                FireEvent(NavigationEventType.MoveToSpecific, current, false);
            }
            else
            {
                SetFork(NavigationState.AtForwardFork, sg.Next[current]);
                FireEvent(NavigationEventType.MoveToNextFork, current, false);
            }
        }

        public bool CanGoPrevious => sg.Previous[current].Any();

        public void GoPrevious()
        {
            var candidateList = sg.Previous[current];
            if (candidateList.Count == 0)
                throw new InvalidOperationException();

            previousStack.Push(current);
            if (candidateList.Count == 1)
            {
                SetFork(NavigationState.AtNode, null);
                current = candidateList.Single();
                FireEvent(NavigationEventType.MoveToSpecific, current, false);
            }
            else
            {
                SetFork(NavigationState.AtBackwardFork, sg.Previous[current]);
                FireEvent(NavigationEventType.MoveToPrevFork, current, false);
            }
        }

        public bool CanGoUp => current != sg.Root;

        public void GoUp()
        {
            if (current == sg.Root)
                return;

            previousStack.Push(current);
            SetFork(NavigationState.AtNode, null);
            current = sg.Parents[current];
            FireEvent(NavigationEventType.MoveToSpecific, current, false);
        }

        public bool CanGoForward => nextStack.Any();

        public void GoForward()
        {
            if (!CanGoForward)
                throw new InvalidOperationException();
            previousStack.Push(current);
            current = nextStack.Pop();
            SetFork(NavigationState.AtNode, null);
            FireEvent(NavigationEventType.MoveToSpecific, current, false);
        }

        public bool CanGoBack => previousStack.Any();

        public void GoBack()
        {
            if (!CanGoBack)
                return;
            nextStack.Push(current);
            current = previousStack.Pop();
            SetFork(NavigationState.AtNode, null);
            FireEvent(NavigationEventType.MoveToSpecific, current, false);
        }

        public void GoForOption(int? option)
        {
            if (!option.HasValue)
            {
                if (AtFork)
                    GoBack();
            }
            else
            {
                GoToSpecific(optionIds[option.Value]);
            }
        }

        public void GoToSpecific(int id)
        {
            GoToSpecificInternal(id, false);
        }

        private void GoToSpecificInternal(int id, bool causedByFocusing)
        {
            nextStack.Clear();
            previousStack.Push(current);
            current = id;
            SetFork(NavigationState.AtNode, null);
            FireEvent(NavigationEventType.MoveToSpecific, current, causedByFocusing);
        }

        public void Reset(int id)
        {
            current = id;
            SetFork(NavigationState.AtNode, null);
            nextStack.Clear();
            previousStack.Clear();
            FireEvent(NavigationEventType.Reset, current, false);        }

        public bool TryHandleInput(IInputEventArgs args)
        {
            if (!(args is KeyEventArgs e) || e.ComplexEventType != KeyEventType.Down)
                return false;

            if (e.EventKey == Key.Right)
            {
                if (e.KeyModifyers.HasFlag(KeyModifyers.Control))
                {
                    if (CanGoForward)
                        GoForward();
                }
                else
                {
                    if (CanGoNext)
                        GoNext();
                }
                return true;
            }
            
            if (e.EventKey == Key.Left)
            {
                if (e.KeyModifyers.HasFlag(KeyModifyers.Control))
                {
                    if (CanGoBack)
                        GoBack();
                }
                else
                {
                    if (CanGoPrevious)
                        GoPrevious();
                }
                return true;
            }
            
            if (e.EventKey == Key.Up)
            {
                if (CanGoUp)
                    GoUp();
                return true;
            }
            
            //if (e.EventKey == Key.Down)
            //{
            //    CurrentInternal = CurrentInternal;
            //    return true;
            //}

            return false;
        }

        private void SetFork(NavigationState state, IReadOnlyList<int> options)
        {
            State = state;
            if (options == null)
            {
                ForkOptions = EmptyArrays<string>.Array;
                optionIds = EmptyArrays<int>.Array;
            }
            else
            {
                ForkOptions = options.Select(x => sg.NodeObjects[x].Name).ToArray();
                optionIds = options.Select(x => sg.NodeObjects[x].Id).ToArray();
            }
        }

        private void FireEvent(NavigationEventType type, int id, bool causedByFocusing)
        {
            var moveInstantly = type == NavigationEventType.Reset ||
                                type == NavigationEventType.MoveToSpecific && sg.Aspects[id].InstantTransition;
            Updated?.Invoke(new NavigationEventArgs(type, moveInstantly, causedByFocusing));
        }
    }
}