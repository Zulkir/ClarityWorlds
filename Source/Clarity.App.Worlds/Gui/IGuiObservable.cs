namespace Clarity.App.Worlds.Gui
{
    public interface IGuiObservable<out TObservable, out TEventArgs>
    {
        void SetObserver(IGuiObserver<TObservable, TEventArgs> observer);
    }
}