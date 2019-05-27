using System;
using System.Collections.Concurrent;
using Clarity.Common.Infra.TreeReadWrite;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Serialization;

namespace Clarity.App.Worlds.Configuration
{
    public class ConfigService : IConfigService
    {
        private readonly IConfigFileStorage fileStorage;
        private readonly ITrwFactory trwFactory;
        private readonly ISerializationNecessities serializationNecessities;

        private readonly ConcurrentDictionary<Type, IConfig> configs;

        public ConfigService(IConfigFileStorage fileStorage, ITrwFactory trwFactory, ISerializationNecessities serializationNecessities)
        {
            this.fileStorage = fileStorage;
            this.serializationNecessities = serializationNecessities;
            this.trwFactory = trwFactory;
            configs = new ConcurrentDictionary<Type, IConfig>();
        }

        public T GetConfig<T>() where T : class, IConfig, new()
        {
            return (T)configs.GetOrAdd(typeof(T), x => LoadConfig<T>());
        }

        private T LoadConfig<T>() where T : class, IConfig, new()
        {
            var handlerContainer = serializationNecessities.GetTrwHandlerContainer(ConfigSerializationNecessitiesProvider.SerializationType);
            var options = new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.Never
            };
            using (var fileStream = fileStorage.FileSystem.OpenRead(GetFileName<T>()))
            using (var trwReader = trwFactory.JsonReader(fileStream))
            using (var context = new TrwSerializationReadContext(trwReader, handlerContainer, null, options))
                return context.Read<T>();
        }

        public void SaveConfig<T>() where T : class, IConfig, new()
        {
            var config = (T)configs.GetOrAdd(typeof(T), x => new T());
            var handlerContainer = serializationNecessities.GetTrwHandlerContainer(ConfigSerializationNecessitiesProvider.SerializationType);
            var typeRedirects = serializationNecessities.GetTrwHandlerTypeRedirects(ConfigSerializationNecessitiesProvider.SerializationType);
            var options = new TrwSerializationOptions
            {
                ExplicitTypes = TrwSerializationExplicitTypes.Never
            };
            using (var fileStream = fileStorage.FileSystem.OpenWriteNew(GetFileName<T>()))
            using (var trwReader = trwFactory.JsonWriter(fileStream))
            using (var context = new TrwSerializationWriteContext(trwReader, handlerContainer, typeRedirects, options))
                context.Write<T>(config);
        }

        private static string GetFileName<T>() => typeof(T).FullName + ".cfg";
    }
}