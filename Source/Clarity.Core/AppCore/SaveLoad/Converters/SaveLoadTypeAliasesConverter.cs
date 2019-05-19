using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public class SaveLoadTypeAliasesConverter : SaveLoadConverterReaderBase
    {
        private readonly ITrwReader previous;
        private readonly IReadOnlyDictionary<string, SaveLoadRenamedTypeDescription> typeRenames;

        private string oldName;

        public SaveLoadTypeAliasesConverter(ITrwReader previous, IReadOnlyDictionary<string, SaveLoadRenamedTypeDescription> typeRenames)
        {
            this.previous = previous;
            this.typeRenames = typeRenames;
        }

        protected override ITrwReader GetImmediatePrevious()
        {
            return previous;
        }

        public override bool MoveNext()
        {
            if (!base.MoveNext())
                return false;
            if (TokenType == TrwTokenType.PropertyName)
                oldName = base.ValueAsString;
            return true;
        }
        
        public override string ValueAsString
        {
            get
            {
                if (!typeRenames.TryGetValue(oldName, out var renamedTypeDesc))
                    return base.ValueAsString;
                return TokenType == TrwTokenType.PropertyName
                    ? renamedTypeDesc.Name
                    : renamedTypeDesc.AssemblyQualifiedName;
            }
        }

    }
}
