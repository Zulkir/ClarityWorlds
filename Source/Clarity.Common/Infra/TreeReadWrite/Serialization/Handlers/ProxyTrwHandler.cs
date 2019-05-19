using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class ProxyTrwHandler<TNew, TExising> : TrwSerializationHandlerBase<TNew>
    {
        private readonly Func<TNew, TExising> new2exising;
        private readonly Func<TExising, TNew> exising2new;

        public override bool ContentIsProperties { get; }

        public ProxyTrwHandler(Func<TNew, TExising> new2Exising, Func<TExising, TNew> exising2New, bool contentIsProperties)
        {
            new2exising = new2Exising;
            exising2new = exising2New;
            ContentIsProperties = contentIsProperties;
        }

        public override void SaveContent(ITrwSerializationWriteContext context, TNew value) => context.Write(new2exising(value));
        public override TNew LoadContent(ITrwSerializationReadContext context) => exising2new(context.Read<TExising>());
    }
}