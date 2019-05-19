using System.Dynamic;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public interface ISaveLoadConverter
    {
        void ChainFrom(ITrwReader reader);
        void ChainFrom(ISaveLoadConverter converter);

        ITrwReader GetResultAsReader();
        ExpandoObject GetResultAsExpando();
    }
}