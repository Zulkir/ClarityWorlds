using System.Collections.Generic;
using Clarity.App.Worlds.Assets;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions
{
    public class AssetTrwHandler : TrwSerializationHandlerBase<IAsset>
    {
        public override bool ContentIsProperties => true;

        public override void SaveContent(ITrwSerializationWriteContext context, IAsset value)
        {
            context.WriteProperty("Name", value.Name);
            var assetDict = (IDictionary<string, IAsset>)context.Bag[SaveLoadConstants.AssetDictBagKey];
            assetDict[value.Name] = value;
        }

        public override IAsset LoadContent(ITrwSerializationReadContext context)
        {
            var name = context.ReadProperty<string>("Name");
            var assetDict = (IDictionary<string, IAsset>)context.Bag[SaveLoadConstants.AssetDictBagKey];
            return assetDict[name];
        }
    }
}