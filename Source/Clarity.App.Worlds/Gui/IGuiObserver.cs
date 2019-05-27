namespace Clarity.App.Worlds.Gui
{
    public interface IGuiObserver<in TObservable, in TEventArgs>
    {
        void OnEvent(TObservable sender, TEventArgs eventArgs);
    }
}