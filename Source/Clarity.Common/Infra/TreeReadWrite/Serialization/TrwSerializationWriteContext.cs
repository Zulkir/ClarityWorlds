using System;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public class TrwSerializationWriteContext : ITrwSerializationWriteContext
    {
        public ITrwWriter Writer { get; }
        public IReadOnlyDictionary<Type, string> TypeAliases => typeAliases;

        private readonly ITrwSerializationHandlerContainer handlers;
        private readonly IReadOnlyList<ITrwSerializationTypeRedirect> typeRedirects;
        private readonly TrwSerializationOptions options;
        private readonly Dictionary<Type, string> typeAliases;
        private readonly HashSet<string> typeAliasesHashSet;

        public IDictionary<string, object> Bag { get; }

        public TrwSerializationWriteContext(ITrwWriter stream, ITrwSerializationHandlerContainer handlers,
            IReadOnlyList<ITrwSerializationTypeRedirect> typeRedirects, TrwSerializationOptions options)
        {
            Writer = stream;
            this.handlers = handlers;
            this.typeRedirects = typeRedirects;
            this.options = options;
            typeAliases = new Dictionary<Type, string>();
            typeAliasesHashSet = new HashSet<string>();
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
            Writer.WriteValue().String(options.AliasTypes ? GetTypeAlias(value) : value.AssemblyQualifiedName);
        }

        private string GetTypeAlias(Type value)
        {
            return typeAliases.GetOrAdd(value, BuildAlias);
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
            while (typeAliasesHashSet.Contains(candidate))
            {
                candidate = baseName + counter;
                counter++;
            }
            typeAliasesHashSet.Add(candidate);
            return candidate;
        }
    }
}