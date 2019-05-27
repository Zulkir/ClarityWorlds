namespace Clarity.Common.Infra.TreeReadWrite.DiffBuilding
{
    public interface ITrwDiffBuilder
    {
        ITrwDiff BuildDiffs(object oldDynValue, object newDynValue, 
                            ITrwDiffIdentityComparer identityComparer);
    }
}