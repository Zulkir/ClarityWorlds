using System.Collections.Generic;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public static class TrwWriterExtensions
    {
        public static void WriteDynamic(this ITrwWriter writer, object obj)
        {
            switch (obj)
            {
                case null:
                    writer.WriteValue().Null();
                    break;
                case bool b:
                    writer.WriteValue().Bool(b);
                    break;
                case int i:
                    writer.WriteValue().SInt32(i);
                    break;
                case double d:
                    writer.WriteValue().Float64(d);
                    break;
                case string s:
                    writer.WriteValue().String(s);
                    break;
                case IEnumerable<KeyValuePair<string, object>> dict:
                    writer.StartObject();
                    foreach (var kvp in dict)
                    {
                        writer.AddProperty(kvp.Key);
                        writer.WriteDynamic(kvp.Value);
                    }
                    writer.EndObject();
                    break;
                case IEnumerable<object> list:
                    writer.StartArray(TrwValueType.Undefined);
                    foreach (var item in list)
                        writer.WriteDynamic(item);
                    writer.EndArray();
                    break;
            }
        }
    }
}