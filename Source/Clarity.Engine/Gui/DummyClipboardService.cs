namespace Clarity.Engine.Gui
{
    public class DummyClipboardService : IClipboardService
    {
        public void CopyString(string str)
        {
            
        }

        public bool TryGetString(out string str)
        {
            str = null;
            return false;
        }
    }
}