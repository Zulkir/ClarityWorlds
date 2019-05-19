using Clarity.Core.AppCore.SaveLoad.Converters.Data;
using Clarity.Core.AppCore.SaveLoad.Converters.Tasks;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public class SaveLoadConverter2 : SaveLoadVersionConverterBase
    {
        public SaveLoadConverter2() 
        {
            AddTask(new TypeRenameConverter(TypeRenames2.Renames));
        }
    }
}