namespace Clarity.Core.AppCore.Gui
{
    public interface IGuiObserver<in TObservable, in TEventArgs>
    {
        void OnEvent(TObservable sender, TEventArgs eventArgs);
    }
}