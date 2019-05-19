namespace Clarity.Core.AppCore.Gui
{
    public interface IGuiObservable<out TObservable, out TEventArgs>
    {
        void SetObserver(IGuiObserver<TObservable, TEventArgs> observer);
    }
}