using System.Collections.Generic;
using System.Diagnostics;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.Handlers
{
    public class StringDictionaryTrwHandler<TDict, TValue> : TrwSerializationHandlerBase<TDict>
        where TDict : IEnumerable<KeyValuePair<string, TValue>>
    {
        public override bool ContentIsProperties => true;

        public StringDictionaryTrwHandler()
        {
            Debug.Assert(typeof(TDict).IsAssignableFrom(typeof(Dictionary<string, TValue>)),
                "typeof(TDict).IsAssignableFrom(typeof(Dictionary<string, TValue>))");
        }

        public override void SaveContent(ITrwSerializationWriteContext context, TDict value)
        {
            foreach (var kvp in value)
            {
                context.Writer.AddProperty(kvp.Key);
                context.Write(typeof(TValue), kvp.Value);
            }
        }

        public override TDict LoadContent(ITrwSerializationReadContext context)
        {
            var dict = new Dictionary<string, TValue>();
            while (context.Reader.TokenType != TrwTokenType.EndObject)
            {
                var key = context.Reader.ValueAsString;
                context.Reader.MoveNext();
                var val = context.Read<TValue>();
                dict.Add(key, val);
            }
            return (TDict)(object)dict;
        }
    }
}