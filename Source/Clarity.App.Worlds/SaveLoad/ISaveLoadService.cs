using Clarity.App.Worlds.SaveLoad.Import;

namespace Clarity.App.Worlds.SaveLoad
{
    public interface ISaveLoadService
    {
        string FileName { get; set; }
        ISaveLoadFormat Format { get; set; }
        bool HasUnsavedChanges { get; }
        void New();
        void Save(SaveWorldFlags worldFlags);
        void Load(LoadWorldPreference worldPreference);
        void Import(IPresentationImporter importer, string filePath, LoadWorldPreference preference);
    }
}