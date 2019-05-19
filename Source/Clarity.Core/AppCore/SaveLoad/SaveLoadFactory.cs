using System;
using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Serialization;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class SaveLoadFactory : ISaveLoadFactory
    {
        private readonly ISerializationNecessities serializationNecessities;

        public SaveLoadFactory(ISerializationNecessities serializationNecessities)
        {
            this.serializationNecessities = serializationNecessities;
        }

        public ITrwSerializationWriteContext MetaWriteContext(ITrwWriter writer) => BasicWriteContext(writer);
        public ITrwSerializationReadContext MetaReadContext(ITrwReader reader) => BasicReadContext(reader);
        public ITrwSerializationWriteContext AliasesWriteContext(ITrwWriter writer) => BasicWriteContext(writer);
        public ITrwSerializationReadContext AliasesReadContext(ITrwReader reader) => BasicReadContext(reader);
        public ITrwSerializationWriteContext AssetsInfoWriteContext(ITrwWriter writer) => BasicWriteContext(writer);
        public ITrwSerializationReadContext AssetsInfoReadContext(ITrwReader reader) => BasicReadContext(reader);

        private ITrwSerializationWriteContext BasicWriteContext(ITrwWriter writer)
        {
            var serializationType = SaveLoadConstants.BasicSerializationType;
            var handlers = serializationNecessities.GetTrwHandlerContainer(serializationType);
            var typeRedirects = serializationNecessities.GetTrwHandlerTypeRedirects(serializationType);
            return new TrwSerializationWriteContext(writer, handlers, typeRedirects, new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.Never
            });
        }

        private ITrwSerializationReadContext BasicReadContext(ITrwReader reader)
        {
            var serializationType = SaveLoadConstants.BasicSerializationType;
            var handlers = serializationNecessities.GetTrwHandlerContainer(serializationType);
            return new TrwSerializationReadContext(reader, handlers, null, new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.Never
            });
        }

        public ITrwSerializationWriteContext WorldWriteContext(ITrwWriter writer)
        {
            var serializationType = SaveLoadConstants.WorldSerializationType;
            var handlers = serializationNecessities.GetTrwHandlerContainer(serializationType);
            var typeRedirects = serializationNecessities.GetTrwHandlerTypeRedirects(serializationType);
            return new TrwSerializationWriteContext(writer, handlers, typeRedirects, new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.WhenAmbiguousOrObject,
                AliasTypes = true,
                TypePropertyName = "type",
                ValuePropertyName = "value"
            });
        }

        public ITrwSerializationReadContext WorldReadContext(ITrwReader reader, IReadOnlyDictionary<string, Type> typeAliases)
        {
            var serializationType = SaveLoadConstants.WorldSerializationType;
            var handlers = serializationNecessities.GetTrwHandlerContainer(serializationType);
            return new TrwSerializationReadContext(reader, handlers, typeAliases, new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.WhenAmbiguousOrObject,
                AliasTypes = true,
                TypePropertyName = "type",
                ValuePropertyName = "value"
            });
        }
    }
}