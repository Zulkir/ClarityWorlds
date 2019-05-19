using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public interface ISaveLoadConverterContainer
    {
        int CurrentVersion { get; }
        ITrwReader ConvertAssetInfoReader(ITrwReader reader, int fromVersion, int toVersion);
        ITrwReader ConvertAliasesReader(ITrwReader reader, int fromVersion, int toVersion);
        ITrwReader ConvertWorldReader(ITrwReader reader, int fromVersion, int toVersion);
    }
}