﻿namespace Clarity.Core.AppCore.SaveLoad
{
    public interface ISaveLoadFormat
    {
        string Name { get; }
        string FileExtension { get; }
        void Save(IFileSaveInfo fileSaveInfo);
        void Load(IFileLoadInfo fileLoadInfo);
    }
}