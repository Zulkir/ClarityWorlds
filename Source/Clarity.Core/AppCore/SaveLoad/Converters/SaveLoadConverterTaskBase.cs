using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public abstract class SaveLoadConverterTaskBase : SaveLoadConverterReaderBase, ISaveLoadConverterTask
    {
        public ITrwReader Previous { get; set; }

        protected override ITrwReader GetImmediatePrevious() => Previous;
    }
}