using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.App.Worlds.SaveLoad.Converters
{
    public interface ISaveLoadConverterTask : ISaveLoadConverter, ITrwReader
    {
        ITrwReader Previous { get; set; }
    }
}