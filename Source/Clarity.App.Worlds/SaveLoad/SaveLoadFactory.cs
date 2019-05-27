using System;
using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Serialization;

namespace Clarity.App.Worlds.SaveLoad
{
    public class SaveLoadFactory : ISaveLoadFactory
    {
        private readonly ISerializationNecessities serializationNecessities;

        public SaveLoadFactory(ISerializationNecessities serializationNecessities)
        {
            this.serializationNecessities = serializationNecessities;
        }

        public ITrwSerializationWriteContext MetaWriteContext(ITrwWriter writer) => BasicWriteContext(writer, false);
        public ITrwSerializationWriteContext AliasesWriteContext(ITrwWriter writer) => BasicWriteContext(writer, false);
        public ITrwSerializationWriteContext AssetsInfoWriteContext(ITrwWriter writer) => BasicWriteContext(writer, true);
        public ITrwSerializationWriteContext GeneratedResourceInfoWriteContext(ITrwWriter writer) => BasicWriteContext(writer, true);

        public ITrwSerializationReadContext MetaReadContext(ITrwReader reader) => BasicReadContext(reader, null);
        public ITrwSerializationReadContext AliasesReadContext(ITrwReader reader) => BasicReadContext(reader, null);
        public ITrwSerializationReadContext AssetsInfoReadContext(ITrwReader reader, IReadOnlyDictionary<string, Type> typeAliases) => BasicReadContext(reader, typeAliases);
        public ITrwSerializationReadContext GeneratedResourceInfoReadContext(ITrwReader reader, IReadOnlyDictionary<string, Type> typeAliases) => BasicReadContext(reader, typeAliases);

        private ITrwSerializationWriteContext BasicWriteContext(ITrwWriter writer, bool aliasTypes)
        {
            var serializationType = SaveLoadConstants.BasicSerializationType;
            var handlers = serializationNecessities.GetTrwHandlerContainer(serializationType);
            var typeRedirects = serializationNecessities.GetTrwHandlerTypeRedirects(serializationType);
            return new TrwSerializationWriteContext(writer, handlers, typeRedirects, new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.Never,
                AliasTypes = aliasTypes
            });
        }

        private ITrwSerializationReadContext BasicReadContext(ITrwReader reader, IReadOnlyDictionary<string, Type> typeAliases)
        {
            var serializationType = SaveLoadConstants.BasicSerializationType;
            var handlers = serializationNecessities.GetTrwHandlerContainer(serializationType);
            return new TrwSerializationReadContext(reader, handlers, typeAliases, new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.Never,
                AliasTypes = typeAliases != null,
            });
        }

        public ITrwSerializationWriteContext WorldWriteContext(ITrwWriter writer)
        {
            var serializationType = SaveLoadConstants.WorldSerializationType;
            var handlers = serializationNecessities.GetTrwHandlerContainer(serializationType);
            var typeRedirects = serializationNecessities.GetTrwHandlerTypeRedirects(serializationType);
            return new TrwSerializationWriteContext(writer, handlers, typeRedirects, new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.WhenObject,
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
                ExplicitTypes = TrwSerializationExplicitTypes.WhenObject,
                AliasTypes = true,
                TypePropertyName = "type",
                ValuePropertyName = "value"
            });
        }
    }
}