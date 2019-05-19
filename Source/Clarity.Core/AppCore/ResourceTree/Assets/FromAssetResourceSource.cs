﻿using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;

namespace Clarity.Core.AppCore.ResourceTree.Assets
{
    [TrwSerialize]
    public class FromAssetResourceSource : IResourceSource
    {
        [TrwSerialize]
        public IAsset Asset { get; private set; }

        public FromAssetResourceSource(IAsset asset)
        {
            Asset = asset;
        }

        public IResource GetResource() => Asset.Resource;
    }
}