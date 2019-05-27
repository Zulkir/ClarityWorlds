using Clarity.Engine.Objects.WorldTree;
using Clarity.Engine.Visualization.Views;

namespace Clarity.App.Worlds.Views 
{
    // todo: consider removing
    public interface IFocusableView : IView
    {
        ISceneNode FocusNode { get; }
        void FocusOn(IFocusNodeComponent cFocusNode);
    }
}