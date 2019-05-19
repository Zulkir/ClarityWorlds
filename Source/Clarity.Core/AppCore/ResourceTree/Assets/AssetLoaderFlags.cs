using System;
using Clarity.Common.CodingUtilities;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    [Flags]
    public enum AssetLoaderFlags
    {
        None = 0,
        MultiFile = 0x1,
        ManualCaching = 0x2
    }

    public static class AssetLoaderFlagsExtensions
    {
        public static bool HasFlag(this AssetLoaderFlags flags, AssetLoaderFlags flag) =>
            CodingHelper.HasFlag((int)flags, (int)flag);
    }
}