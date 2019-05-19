namespace Clarity.Core.AppCore.SaveLoad
{
    public interface ISaveLoadService
    {
        string FileName { get; set; }
        ISaveLoadFormat Format { get; set; }
        bool HasUnsavedChanges { get; }
        void New();
        void Save();
        void Load();
    }
}