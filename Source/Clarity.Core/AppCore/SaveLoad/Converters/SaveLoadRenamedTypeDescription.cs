namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public struct SaveLoadRenamedTypeDescription
    {
        public string Name;
        public string AssemblyQualifiedName;

        public SaveLoadRenamedTypeDescription(string name, string assemblyQualifiedName)
        {
            Name = name;
            AssemblyQualifiedName = assemblyQualifiedName;
        }
    }
}