namespace Clarity.Core.AppCore.Tools
{
    public class ToolService : IToolService
    {
        private ITool currentTool;
        
        public ITool CurrentTool
        {
            get { return currentTool; }
            set { currentTool?.Dispose(); currentTool = value; }
        }
    }
}