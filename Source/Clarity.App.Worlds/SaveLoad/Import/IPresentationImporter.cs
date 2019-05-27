using System.Collections.Generic;

namespace Clarity.App.Worlds.SaveLoad.Import
{
    public interface IPresentationImporter
    {
        string Name { get; }
        IReadOnlyList<string> FileExtensions { get; }
        void Load(IFileLoadInfo fileLoadInfo);
    }
}