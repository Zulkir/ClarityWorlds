using System;
using System.IO;
using Newtonsoft.Json;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Json
{
    public class TrwReaderJson : ITrwReader
    {
        private readonly JsonTextReader reader;

        public TrwTokenType TokenType { get; private set; }

        public TrwReaderJson(Stream stream)
        {
            reader = new JsonTextReader(new StreamReader(stream))
            {
                CloseInput = true
            };
        }

        public void Dispose()
        {
            reader.Close();
        }

        public bool MoveNext()
        {
            var result = reader.Read();
            TokenType = ConvertTokenType(reader.TokenType);
            return result;
        }

        private TrwTokenType ConvertTokenType(JsonToken jsonToken)
        {
            switch (jsonToken)
            {
                case JsonToken.None: return TrwTokenType.None;
                case JsonToken.StartObject: return TrwTokenType.StartObject;
                case JsonToken.StartArray: return TrwTokenType.StartArray;
                case JsonToken.PropertyName: return TrwTokenType.PropertyName;
                case JsonToken.Integer: return TrwTokenType.Integer;
                case JsonToken.Float: return TrwTokenType.Float;
                case JsonToken.String: return TrwTokenType.String;
                case JsonToken.Boolean: return TrwTokenType.Boolean;
                case JsonToken.Null: return TrwTokenType.Null;
                case JsonToken.EndObject: return TrwTokenType.EndObject;
                case JsonToken.EndArray: return TrwTokenType.EndArray;
                default: throw new ArgumentOutOfRangeException(nameof(jsonToken), jsonToken, null);
            }
        }

        public void Skip()
        {
            reader.Skip();
            reader.Read();
            TokenType = ConvertTokenType(reader.TokenType);
        }
        
        public bool ValueAsBool => (bool)reader.Value;
        public int ValueAsSInt32 => Convert.ToInt32(reader.Value);
        public string ValueAsString => Convert.ToString(reader.Value);
        public double ValueAsFloat64 => Convert.ToSingle(reader.Value);

        public int LineNumber => reader.LineNumber;
        public int LinePosition => reader.LinePosition;
        public string CurrentEntryPath => reader.Path;// pathBuilder.BuildPath();
    }
}
