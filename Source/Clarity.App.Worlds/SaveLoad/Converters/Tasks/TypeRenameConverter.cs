using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.App.Worlds.SaveLoad.Converters.Tasks
{
    public class TypeRenameConverter : SaveLoadConverterTaskBase
    {
        private readonly IReadOnlyDictionary<string, SaveLoadRenamedTypeDescription> renames;

        public TypeRenameConverter(IReadOnlyDictionary<string ,SaveLoadRenamedTypeDescription> renames) 
        {
            this.renames = renames;
        }

        public override string ValueAsString
        {
            get
            {
                if (TokenType == TrwTokenType.String && LastProperty == "@type")
                    return GetNewName(Previous.ValueAsString);
                if (TokenType == TrwTokenType.String && GetTypeName() == "Type" && LastProperty == "@value")
                    return GetNewName(Previous.ValueAsString);
                return base.ValueAsString;
            }
        }

        private string GetNewName(string oldName)
        {
            return renames.TryGetValue(oldName, out var newTypeDesc) ? newTypeDesc.Name : oldName;
        }
    }
}