using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.App.Worlds.SaveLoad.Converters
{
    public interface ISaveLoadConverterContainer
    {
        int CurrentVersion { get; }
        ITrwReader ConvertAssetInfoReader(ITrwReader reader, int fromVersion, int toVersion);
        ITrwReader ConvertGeneratedResourceInfoReader(ITrwReader reader, int fromVersion, int toVersion);
        ITrwReader ConvertAliasesReader(ITrwReader reader, int fromVersion, int toVersion);
        ITrwReader ConvertWorldReader(ITrwReader reader, int fromVersion, int toVersion);
    }
}