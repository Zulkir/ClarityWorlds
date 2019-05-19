using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public static class TrwReaderExtensions
    {
        #region Check
        public static void Check(this ITrwReader reader, TrwTokenType tokenType)
        {
            if (reader.TokenType != tokenType)
                throw new Exception($"'{tokenType}' was expected, but '{reader.TokenType}' found. (Ln {reader.LineNumber} Col {reader.LinePosition})");
        }

        public static void MoveNextAndCheck(this ITrwReader reader, TrwTokenType tokenType)
        {
            reader.MoveNext();
            reader.Check(tokenType);
        }

        public static void CheckAndMoveNext(this ITrwReader reader, TrwTokenType tokenType)
        {
            reader.Check(tokenType);
            reader.MoveNext();
        }

        public static void CheckProperty(this ITrwReader reader, string propertyName)
        {
            reader.Check(TrwTokenType.PropertyName);
            if (reader.ValueAsString != propertyName)
                throw new Exception($"Property '{propertyName}' was expected, but '{reader.ValueAsString}' found. (Ln {reader.LineNumber} Col {reader.LinePosition})");
        }

        public static void CheckPropertyAndMoveNext(this ITrwReader reader, string propertyName)
        {
            reader.CheckProperty(propertyName);
            reader.MoveNext();
        }
        #endregion

        #region Dynamic
        public static dynamic ReadAsDynamic(this ITrwReader reader)
        {
            if (!reader.MoveNext())
                return null;
            return ReadValue(reader);
        }

        private static dynamic ReadObject(ITrwReader reader)
        {
            var result = new ExpandoObject() as IDictionary<string, object>;
            var propertyName = (string)null;
            while (reader.MoveNext())
            {
                switch (reader.TokenType)
                {
                    case TrwTokenType.PropertyName:
                        propertyName = reader.ValueAsString;
                        break;
                    case TrwTokenType.EndObject:
                        return result;
                    case TrwTokenType.StartObject:
                    case TrwTokenType.StartArray:
                    case TrwTokenType.Null:
                    case TrwTokenType.Boolean:
                    case TrwTokenType.Integer:
                    case TrwTokenType.Float:
                    case TrwTokenType.String:
                        Debug.Assert(propertyName != null, nameof(propertyName) + " != null");
                        result[propertyName] = ReadValue(reader);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return result;
        }

        private static List<dynamic> ReadArray(ITrwReader reader)
        {
            var result = new List<object>();
            while (reader.MoveNext())
            {
                switch (reader.TokenType)
                {
                    case TrwTokenType.StartObject:
                    case TrwTokenType.StartArray:
                    case TrwTokenType.Null:
                    case TrwTokenType.Boolean:
                    case TrwTokenType.Integer:
                    case TrwTokenType.Float:
                    case TrwTokenType.String:
                        result.Add(ReadValue(reader));
                        break;
                    case TrwTokenType.EndArray:
                        return result;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return result;
        }

        private static object ReadValue(ITrwReader reader)
        {
            switch (reader.TokenType)
            {
                case TrwTokenType.StartObject: return ReadObject(reader);
                case TrwTokenType.StartArray: return ReadArray(reader);
                case TrwTokenType.Null: return null;
                case TrwTokenType.Boolean: return reader.ValueAsBool;
                case TrwTokenType.Integer: return reader.ValueAsSInt32;
                case TrwTokenType.Float: return reader.ValueAsFloat64;
                case TrwTokenType.String: return reader.ValueAsString;
                default: throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}