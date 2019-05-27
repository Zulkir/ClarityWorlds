using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public class TrwSerializationWriteContext : ITrwSerializationWriteContext
    {
        public ITrwWriter Writer { get; }
        public IDictionary<Type, string> TypeAliases { get; set; }
        public IDictionary<string, object> Bag { get; }

        private readonly ITrwSerializationHandlerContainer handlers;
        private readonly IReadOnlyList<ITrwSerializationTypeRedirect> typeRedirects;
        private readonly TrwSerializationOptions options;

        public TrwSerializationWriteContext(ITrwWriter stream, ITrwSerializationHandlerContainer handlers,
            IReadOnlyList<ITrwSerializationTypeRedirect> typeRedirects, TrwSerializationOptions options)
        {
            Writer = stream;
            TypeAliases = new Dictionary<Type, string>();
            this.handlers = handlers;
            this.typeRedirects = typeRedirects;
            this.options = options;
            Bag = new Dictionary<string, object>();
        }

        public void Dispose()
        {
        }

        public void Write<T>(T value) => WriteInternal(value, typeof(T));
        public void Write(Type type, object value) => WriteInternal(value, type);

        private void WriteInternal<T>(T value, Type explicitType)
        {
            if (value == null)
            {
                Writer.WriteValue().Null();
                return;
            }

            var ambiguous = !explicitType.IsSealed;
            var type = ResolveRedirect(ambiguous ? value.GetType() : explicitType);
            var handler = handlers.GetHandler(type);
            var typedHandler = handler as ITrwSerializationHandler<T>;

            void WriteContentWithHandler()
            {
                if (typedHandler != null)
                    typedHandler.SaveContent(this, value);
                else
                    handler.SaveContent(this, value);
            }

            bool writeType;
            switch (options.ExplicitTypes)
            {
                case TrwSerializationExplicitTypes.Never:
                    writeType = false;
                    break;
                case TrwSerializationExplicitTypes.WhenAmbiguous:
                    writeType = ambiguous;
                    break;
                case TrwSerializationExplicitTypes.WhenObject:
                    writeType = handler.ContentIsProperties;
                    break;
                case TrwSerializationExplicitTypes.WhenAmbiguousOrObject:
                    writeType = ambiguous || handler.ContentIsProperties;
                    break;
                case TrwSerializationExplicitTypes.Always:
                    writeType = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (writeType)
            {
                Writer.StartObject();
                Writer.AddProperty(options.TypePropertyName);
                WriteType(type);
                if (!handler.ContentIsProperties)
                    Writer.AddProperty(options.ValuePropertyName);
                WriteContentWithHandler();
                Writer.EndObject();
            }
            else
            {
                if (!handler.ContentIsProperties)
                    WriteContentWithHandler();
                else
                {
                    Writer.StartObject();
                    WriteContentWithHandler();
                    Writer.EndObject();
                }
            }
        }

        public void WriteType(Type value)
        {
            // todo: special case for mem
            Writer.WriteValue().String(options.AliasTypes ? GetTypeAlias(value) : value.AssemblyQualifiedName);
        }

        private string GetTypeAlias(Type type)
        {
            return TypeAliases.GetOrAddEx(type, BuildAlias);
        }

        private Type ResolveRedirect(Type type)
        {
            foreach (var redirect in typeRedirects)
                if (redirect.TryRedirect(type, out var redirectedType))
                    return redirectedType;
            return type;
        }

        private string BuildAlias(Type type)
        {
            var baseName = (type.FullName ?? "UnknownType").Split('.').Last().Replace('+', '_');
            var candidate = baseName;
            var counter = 1;
            while (TypeAliases.Values.Contains(candidate))
            {
                candidate = baseName + counter;
                counter++;
            }
            return candidate;
        }
    }
}