using System.Collections.Generic;
using Clarity.Common.CodingUtilities.Tuples;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.App.Worlds.SaveLoad.Converters.Tasks
{
    public class PropertyRenameConverter : SaveLoadConverterTaskBase
    {
        private readonly IReadOnlyDictionary<Pair<string, string>, string> renames;

        public PropertyRenameConverter(IReadOnlyDictionary<Pair<string, string>, string> renames)
        {
            this.renames = renames;
        }

        public override string ValueAsString
        {
            get
            {
                if (Previous.TokenType != TrwTokenType.PropertyName)
                    return Previous.ValueAsString;
                return renames.TryGetValue(new Pair<string, string>(GetTypeName(), Previous.ValueAsString), out var newName) 
                    ? newName 
                    : Previous.ValueAsString;
            }
        }
    }
}