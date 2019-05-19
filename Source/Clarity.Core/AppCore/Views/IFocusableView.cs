using Clarity.Common.Infra.ActiveModel;
using Clarity.Core.AppCore.WorldTree;
using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Views;

namespace Clarity.Core.AppCore.Views 
{
    public interface IFocusableView : IView
    {
        ISceneNode FocusNode { get; }
        void FocusOn(IFocusNodeComponent cFocusNode);

        // todo: remove from here
        void OnWorldUpdated(IAmEventMessage message);
        void OnNavigationEvent(INavigationEventArgs args);
        void OnQueryServiceUpdated();
    }
}