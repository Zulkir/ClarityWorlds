using System;
using System.Collections.Generic;
using System.IO;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public class TrwSerializationReadContext : ITrwSerializationReadContext
    {
        public ITrwReader Reader { get; }
        public IDictionary<string, object> Bag { get; }
        private readonly ITrwSerializationHandlerContainer handlers;
        private readonly IReadOnlyDictionary<string, Type> typeAliases;
        private readonly TrwSerializationOptions options;

        public TrwSerializationReadContext(ITrwReader reader, ITrwSerializationHandlerContainer handlers, IReadOnlyDictionary<string, Type> typeAliases, TrwSerializationOptions options)
        {
            Reader = reader;
            this.handlers = handlers;
            this.typeAliases = typeAliases;
            this.options = options;
            Bag = new Dictionary<string, object>();
            reader.MoveNext();
        }

        public void Dispose()
        {
        }

        public T Read<T>() => ReadInternal<T>(typeof(T));
        public object Read(Type type) => ReadInternal<object>(type);

        private T ReadInternal<T>(Type givenType)
        {
            if (Reader.TokenType == TrwTokenType.Null)
            {
                Reader.MoveNext();
                return default(T);
            }

            if (Reader.TokenType != TrwTokenType.StartObject)
            {
                if (givenType == typeof(object))
                    throw new InvalidDataException($"No type specified for an ambiguous value. (Ln {Reader.LineNumber} Col {Reader.LinePosition})");
                if (options.ExplicitTypes == TrwSerializationExplicitTypes.Always)
                    throw new InvalidDataException($"ExplicitTypes option is set to Always, but a typeless value found. (Ln {Reader.LineNumber} Col {Reader.LinePosition})");
                return typeof(T) == givenType
                    ? handlers.GetHandler<T>().LoadContent(this)
                    : (T)handlers.GetHandler(givenType).LoadContent(this);
            }

            Reader.MoveNext();

            Type type;
            if (Reader.TokenType == TrwTokenType.PropertyName && Reader.ValueAsString == options.TypePropertyName)
            {
                Reader.MoveNext();
                type = ReadType();
            }
            else
            {
                type = givenType;
            }

            if (type == null)
                throw new InvalidDataException($"No type specified for an ambiguous value. (Ln {Reader.LineNumber} Col {Reader.LinePosition})");

            if (Reader.TokenType == TrwTokenType.PropertyName && Reader.ValueAsString == options.ValuePropertyName)
                Reader.MoveNext();

            var obj = typeof(T) == type
                ? handlers.GetHandler<T>().LoadContent(this)
                : (T)handlers.GetHandler(type).LoadContent(this); ;
            Reader.CheckAndMoveNext(TrwTokenType.EndObject);
            return obj;
        }

        public T ReadFluent<T>(T val)
        {
            Reader.MoveNext();
            return val;
        }

        public Type ReadType()
        {
            var typeStr = Reader.ValueAsString;
            return ReadFluent(options.AliasTypes ? typeAliases[typeStr] : Type.GetType(typeStr));
        }
    }
}