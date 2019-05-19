using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public interface ISaveLoadConverterTask : ISaveLoadConverter, ITrwReader
    {
        ITrwReader Previous { get; set; }
    }
}