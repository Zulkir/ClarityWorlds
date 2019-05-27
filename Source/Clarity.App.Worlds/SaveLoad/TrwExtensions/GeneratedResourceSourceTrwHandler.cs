using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions
{
    public class GeneratedResourceSourceTrwHandler : TrwSerializationHandlerBase<GeneratedResourceSource>
    {
        public override bool ContentIsProperties => true;

        public override void SaveContent(ITrwSerializationWriteContext context, GeneratedResourceSource value)
        {
            var generatedResourceList = (IList<GeneratedResourceSource>)context.Bag[SaveLoadConstants.GeneratedResourcesBagKey];
            var index = generatedResourceList.Count;
            generatedResourceList.Add(value);
            context.WriteProperty("Index", index);
        }

        public override GeneratedResourceSource LoadContent(ITrwSerializationReadContext context)
        {
            var generatedResourceList = (IReadOnlyList<IResource>)context.Bag[SaveLoadConstants.GeneratedResourcesBagKey];
            var index = context.ReadProperty<int>("Index");
            return (GeneratedResourceSource)generatedResourceList[index].Source;
        }
    }
}