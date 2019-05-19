using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class ResourceTrwHandler<T> : TrwSerializationHandlerBase<T>
        where T : IResource
    {
        public override bool ContentIsProperties => true;

        public override void SaveContent(ITrwSerializationWriteContext context, T value)
        {
            context.WriteProperty("Source", value.Source);
        }

        public override T LoadContent(ITrwSerializationReadContext context)
        {
            context.Reader.CheckPropertyAndMoveNext("Source");
            var source = context.Read<IResourceSource>();
            return (T)source.GetResource();
        }
    }
}