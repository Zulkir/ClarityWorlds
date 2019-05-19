using System.Dynamic;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public abstract class SaveLoadConverterExpandoBase : ISaveLoadConverter
    {
        private object source;

        public void ChainFrom(ITrwReader reader) { source = reader; }
        public void ChainFrom(ISaveLoadConverter converter) { source = converter; }

        public ITrwReader GetResultAsReader() { throw new System.NotImplementedException(); }
        public ExpandoObject GetResultAsExpando() { throw new System.NotImplementedException(); }

        protected abstract ExpandoObject Run(ExpandoObject input);
    }
}