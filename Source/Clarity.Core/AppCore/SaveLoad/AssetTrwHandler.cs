using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Core.AppCore.ResourceTree.Assets;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class AssetTrwHandler : TrwSerializationHandlerBase<IAsset>
    {
        public override bool ContentIsProperties => true;

        public override void SaveContent(ITrwSerializationWriteContext context, IAsset value)
        {
            context.WriteProperty("Name", value.Name);
            var assetDict = (IDictionary<string, IAsset>)context.Bag[SaveLoadConstants.AssetDictBagKey];
            assetDict.Add(value.Name, value);
        }

        public override IAsset LoadContent(ITrwSerializationReadContext context)
        {
            var name = context.ReadProperty<string>("Name");
            var assetDict = (IDictionary<string, IAsset>)context.Bag[SaveLoadConstants.AssetDictBagKey];
            return assetDict[name];
        }
    }
}