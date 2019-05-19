using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using Clarity.Common.CodingUtilities.Sugar.Wrappers.Exceptions;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Xml
{
    public class TrwReaderXml : ITrwReader
    {
        private enum ElementType
        {
            Unknown,
            Object,
            Array,
            Value,
        }

        private class ElemInfo
        {
            public ElementType Type { get; set; }
            public string Name { get; }
            public bool IsEmpty { get; }

            public ElemInfo(string name, bool isEmpty)
            {
                Type = ElementType.Unknown;
                Name = name;
                IsEmpty = isEmpty;
            }

            public ElemInfo(ElementType type, string name, bool isEmpty)
            {
                Type = type;
                Name = name;
                IsEmpty = isEmpty;
            }
        }

        private struct TokenInfo
        {
            public TrwTokenType Type;
            public long ValInt;
            public double ValFloat;
            public bool ValBool;
            public string ValString;
            
            public TokenInfo(TrwTokenType type) : this()
            {
                Type = type;
            }

            public TokenInfo(long valInt) : this()
            {
                Type = TrwTokenType.Integer;
                ValInt = valInt;
            }

            public TokenInfo(double valFloat) : this()
            {
                Type = TrwTokenType.Float;
                ValFloat = valFloat;
            }

            public TokenInfo(bool valBool) : this()
            {
                Type = TrwTokenType.Boolean;
                ValBool = valBool;
            }

            public TokenInfo(string valString) : this()
            {
                Type = TrwTokenType.String;
                ValString = valString;
            }
        }

        private readonly XmlReader reader;
        private readonly Stream stream;
        private readonly Stack<ElemInfo> elemStack;
        private readonly Queue<TokenInfo> tokenQueue;
        private readonly TrwPathBuilder pathBuilder;
        private bool lastElemWasEmpty;

        private TokenInfo currentToken;

        public TrwReaderXml(Stream stream)
        {
            this.stream = stream;
            var settings = new XmlReaderSettings();
            reader = XmlReader.Create(stream, settings);
            elemStack = new Stack<ElemInfo>();
            tokenQueue = new Queue<TokenInfo>();
            pathBuilder = new TrwPathBuilder();
        }

        public void Skip()
        {
            // todo: make work
            reader.Skip();
            reader.MoveToContent();
            switch (reader.NodeType)
            {
                case XmlNodeType.None:
                    throw new Exception($"Unexpected node type '{reader.NodeType}'");
                case XmlNodeType.Element:
                    currentToken = new TokenInfo(TrwTokenType.PropertyName) { ValString = reader.Name };
                    break;
            }
            pathBuilder.OnSkip(this);
        }

        public void Dispose()
        {
            reader.Dispose();
            stream.Dispose();
        }

        public bool MoveNext()
        {
            if (!tokenQueue.Any())
            {
                DoRead();
                if (!tokenQueue.Any())
                    return false;
            }
            currentToken = tokenQueue.Dequeue();
            pathBuilder.OnRead(this);
            return true;
        }

        private void DoRead()
        {
            if (currentToken.Type == TrwTokenType.None)
            {
                while (!(reader.NodeType == XmlNodeType.Element && reader.Name == "Root"))
                {
                    reader.Read();
                    if (reader.EOF)
                        throw new Exception("<Root> element was not found.");
                }
                elemStack.Push(new ElemInfo(ElementType.Object, "Root", reader.IsEmptyElement));
                tokenQueue.Enqueue(new TokenInfo(TrwTokenType.StartObject));
                return;
            }

            while (!tokenQueue.Any())
            {
                var readResult = reader.MoveToNextAttribute() || reader.Read();
                if (readResult == false)
                    return;
                switch (reader.NodeType)
                {
                    case XmlNodeType.None:
                        throw new Exception($"Unexpected node type '{reader.NodeType}'");
                    case XmlNodeType.Element:
                        if (lastElemWasEmpty)
                        {
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.StartObject));
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.EndObject));
                        }
                        var prevElement = elemStack.Peek();
                        lastElemWasEmpty = reader.IsEmptyElement;
                        if (!reader.IsEmptyElement)
                            elemStack.Push(new ElemInfo(reader.Name, reader.IsEmptyElement));
                        var elemName = reader.Name;
                        if (prevElement.Type == ElementType.Unknown)
                        {
                            prevElement.Type = ElementType.Object;
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.StartObject));
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.PropertyName) { ValString = elemName });
                        }
                        else if (prevElement.Type == ElementType.Array && elemName == "Elem")
                        {
                        }
                        else
                        {
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.PropertyName) { ValString = elemName });
                        }
                        break;
                    case XmlNodeType.Attribute:
                        var attrName = reader.Name;
                        if (attrName == "null")
                        {
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.Null));
                            lastElemWasEmpty = false;
                        }
                        else if (attrName == "array")
                        {
                            var elem = elemStack.Peek();
                            if (lastElemWasEmpty)
                            {
                                tokenQueue.Enqueue(new TokenInfo(TrwTokenType.StartArray));
                                tokenQueue.Enqueue(new TokenInfo(TrwTokenType.EndArray));
                                lastElemWasEmpty = false;
                            }
                            else
                            {
                                elem.Type = ElementType.Array;
                                tokenQueue.Enqueue(new TokenInfo(TrwTokenType.StartArray));
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        var text = reader.Value;
                        elemStack.Peek().Type = ElementType.Value;
                        if (text.Length >= 2 && text.StartsWith("[") && text.EndsWith("]"))
                        {
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.StartArray));
                            var valueStrings = text.Substring(1, text.Length - 2).Split(',');
                            foreach (var vs in valueStrings)
                                tokenQueue.Enqueue(ParseText(vs.Trim()));
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.EndArray));
                        }
                        else
                            tokenQueue.Enqueue(ParseText(text));
                        break;
                    case XmlNodeType.EndElement:
                        if (lastElemWasEmpty)
                        {
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.StartObject));
                            tokenQueue.Enqueue(new TokenInfo(TrwTokenType.EndObject));
                        }
                        lastElemWasEmpty = reader.IsEmptyElement;
                        var closingElem = elemStack.Pop();
                        var elemType = closingElem.Type;
                        switch (elemType)
                        {
                            case ElementType.Unknown:
                                tokenQueue.Enqueue(new TokenInfo(TrwTokenType.StartObject));
                                tokenQueue.Enqueue(new TokenInfo(TrwTokenType.EndObject));
                                break;
                            case ElementType.Object:
                                tokenQueue.Enqueue(new TokenInfo(TrwTokenType.EndObject));
                                break;
                            case ElementType.Array:
                                tokenQueue.Enqueue(new TokenInfo(TrwTokenType.EndArray));
                                break;
                            case ElementType.Value:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                }
            }
        }

        private static TokenInfo ParseText(string text)
        {
            if (text.Length >= 2 && text.StartsWith("\"") && text.EndsWith("\""))
                return new TokenInfo(text.Substring(1, text.Length - 2));
            if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var valInt))
                return new TokenInfo(valInt);
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var valFloat))
                return new TokenInfo(valFloat);
            if (bool.TryParse(text, out var valBool))
                return new TokenInfo(valBool);
            throw new Exception("Failed to decode text: " + text);
        }

        public TrwTokenType TokenType => currentToken.Type;

        public bool ValueAsBool => 
            TokenType == TrwTokenType.Boolean ? currentToken.ValBool : ExcepResult.New<bool>(new Exception("Trying to decode a non-boolean value as a bool."));

        public int ValueAsSInt32 => 
            TokenType == TrwTokenType.Integer ? (int)currentToken.ValInt : ExcepResult.New<int>(new Exception("Trying to decode a non-integer value as an int32."));

        public string ValueAsString => 
            (TokenType == TrwTokenType.String || TokenType == TrwTokenType.PropertyName) 
                ? currentToken.ValString : ExcepResult.New<string>(new Exception("Trying to decode a non-string value as a string."));

        public double ValueAsFloat64 => 
            TokenType == TrwTokenType.Float ? currentToken.ValFloat :
            TokenType == TrwTokenType.Integer ? currentToken.ValInt : 
            ExcepResult.New<double>(new Exception("Trying to decode a non-numeric value as a float64."));

        public int LineNumber => (reader as IXmlLineInfo)?.LineNumber ?? -1;
        public int LinePosition => (reader as IXmlLineInfo)?.LinePosition ?? -1;
        public string CurrentEntryPath => pathBuilder.BuildPath();
    }
}