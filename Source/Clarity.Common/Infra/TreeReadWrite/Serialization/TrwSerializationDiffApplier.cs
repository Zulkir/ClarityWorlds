using System;
using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;
using Clarity.Common.Infra.TreeReadWrite.Formats.Mem;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public class TrwSerializationDiffApplier : ITrwSerializationDiffApplier
    {
        public const string ValuePropertyName = "@value";
        public const string TypePropertyName = "@type";

        private readonly ITrwSerializationHandlerContainer handlers;
        private readonly IReadOnlyList<ITrwSerializationTypeRedirect> typeRedirects;
        private readonly Action<IDictionary<string, object>> fillWriteBag;
        private readonly Action<IDictionary<string, object>> fillReadBag;
        private readonly TrwSerializationOptions serializationOptions;

        public TrwSerializationDiffApplier(ITrwSerializationHandlerContainer handlers, IReadOnlyList<ITrwSerializationTypeRedirect> typeRedirects, 
            Action<IDictionary<string, object>> fillWriteBag, Action<IDictionary<string, object>> fillReadBag)
        {
            this.handlers = handlers;
            this.typeRedirects = typeRedirects;
            this.fillWriteBag = fillWriteBag;
            this.fillReadBag = fillReadBag;
            serializationOptions = new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.WhenObject,
                ValuePropertyName = ValuePropertyName,
                TypePropertyName = TypePropertyName,
                AliasTypes = false
            };
        }

        public void ApplyDiff<T>(T target, ITrwDiff diff, TrwDiffDirection direction)
        {
            var type = ResolveRedirect(target.GetType());
            handlers.GetHandler(type).ApplyDiff(this, target, diff, direction);
        }

        private Type ResolveRedirect(Type type)
        {
            foreach (var redirect in typeRedirects)
                if (redirect.TryRedirect(type, out var redirectedType))
                    return redirectedType;
            return type;
        }

        public object ToDynamic(object value)
        {
            // todo: make DynamicFormat
            var tokens = new List<TrwMemToken>();
            using (var writer = new TrwWriterMem(x => tokens.Add(x), () => { }))
            using (var writeContext = new TrwSerializationWriteContext(writer, handlers, typeRedirects, serializationOptions))
            {
                fillWriteBag(writeContext.Bag);
                writeContext.Write(value);
            }
            using (var reader = new TrwReaderMem(tokens))
                return reader.ReadAsDynamic();
        }

        public object FromDynamic(Type type, object dynValue)
        {
            // todo: make DynamicFormat
            var tokens = new List<TrwMemToken>();
            using (var tokenWriter = new TrwWriterMem(x => tokens.Add(x), () => { }))
                tokenWriter.WriteDynamic(dynValue);
            using (var reader = new TrwReaderMem(tokens))
            using (var readContext = new TrwSerializationReadContext(reader, handlers, null, serializationOptions))
            {
                fillReadBag(readContext.Bag);
                return readContext.Read(type);
            }
        }
    }
}