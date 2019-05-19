using Clarity.Core.AppCore.SaveLoad.Converters.Data;
using Clarity.Core.AppCore.SaveLoad.Converters.Tasks;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public class SaveLoadConverter3 : SaveLoadVersionConverterBase
    {
        public SaveLoadConverter3()
        {
            AddTask(new TypeRenameConverter(TypeRenames3.Renames));
            AddTask(new PropertyRenameConverter(PropertyRenames3.Renames));
        }
    }
}