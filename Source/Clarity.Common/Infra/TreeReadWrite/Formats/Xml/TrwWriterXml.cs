using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace Clarity.Common.Infra.TreeReadWrite.Formats.Xml
{
    public class TrwWriterXml : ITrwWriter, ITrwValueWriter
    {
        private enum WriterState
        {
            ExpectingStartRootObject,
            ExpectingProperty,
            ExpectingPropertyValue,
            ExpectingArrayElement,
        }

        private static readonly XmlWriterSettings XmlSettings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace,
            Encoding = Encoding.UTF8,
            CloseOutput = true
        };

        private const string ArrayElemStr = "Elem";

        private readonly XmlWriter writer;
        private readonly Stack<WriterState> stateStack = new Stack<WriterState>();
        private readonly StringBuilder structuredArrayBuilder = new StringBuilder();
        private WriterState state = WriterState.ExpectingStartRootObject;
        private readonly Stack<TrwValueType> arrayTypeStack = new Stack<TrwValueType>();
        private readonly TrwPathBuilder pathBuilder = new TrwPathBuilder();

        public TrwWriterXml(Stream stream)
        {
            writer = XmlWriter.Create(stream, XmlSettings);
        }

        public TrwWriterXml(StringBuilder stringBuilder)
        {
            writer = XmlWriter.Create(stringBuilder, XmlSettings);
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        public void AddProperty(string name)
        {
            if (state == WriterState.ExpectingPropertyValue)
                throw new InvalidOperationException("A property value was expected, but AppProperty was called.");
            writer.WriteStartElement(name);
            state = WriterState.ExpectingPropertyValue;
            pathBuilder.OnProperty(name);
        }

        public void StartObject()
        {
            if (state == WriterState.ExpectingStartRootObject)
                writer.WriteStartElement("Root");
            else if (state == WriterState.ExpectingArrayElement)
                writer.WriteStartElement(ArrayElemStr);
            stateStack.Push(state);
            state = WriterState.ExpectingProperty;
            pathBuilder.OnStartObject();
        }

        public void EndObject()
        {
            if (state != WriterState.ExpectingProperty)
                throw new InvalidOperationException("EndObject was called in an unappropriate place.");
            //writer.WriteEndElement();
            writer.WriteEndElement();
            state = stateStack.Pop();
            OnWritingValue();
            pathBuilder.OnEndObject();
        }

        public void StartArray(TrwValueType arrayType)
        {
            if (state == WriterState.ExpectingArrayElement)
                writer.WriteStartElement(ArrayElemStr);
            arrayTypeStack.Push(arrayType);
            if (arrayTypeStack.Peek() == TrwValueType.Undefined)
            {
                writer.WriteAttributeString("array", "True");
            }
            else
            {
                structuredArrayBuilder.Clear();
                structuredArrayBuilder.Append("[");
            }
            stateStack.Push(state);
            state = WriterState.ExpectingArrayElement;
            pathBuilder.OnStartArray();
        }

        public void EndArray()
        {
            if (arrayTypeStack.Peek() != TrwValueType.Undefined)
            {
                structuredArrayBuilder.Append("]");
                writer.WriteString(structuredArrayBuilder.ToString());
            }
            writer.WriteEndElement();
            state = stateStack.Pop();
            arrayTypeStack.Pop();
            OnWritingValue();
            pathBuilder.OnEndArray();
        }

        public ITrwValueWriter WriteValue()
        {
            OnWritingValue();
            return this;
        }

        private void OnWritingValue()
        {
            if (state == WriterState.ExpectingPropertyValue)
                state = WriterState.ExpectingProperty;
        }

        public void Flush()
        {
            writer.Flush();
        }

        public string NextEntryPath => pathBuilder.BuildPath();

        #region Values
        public void Null()
        {
            if (state != WriterState.ExpectingArrayElement)
            {
                writer.WriteAttributeString("null", "True");
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteStartElement(ArrayElemStr);
                writer.WriteAttributeString("null", "True");
                writer.WriteEndElement();
            }
            pathBuilder.OnValue();
        }

        public void String(string val)
        {
            if (state != WriterState.ExpectingArrayElement)
            {
                writer.WriteString($"\"{val}\"");
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteStartElement(ArrayElemStr);
                writer.WriteString($"\"{val}\"");
                writer.WriteEndElement();
            }
            pathBuilder.OnValue();
        }

        public void Bool(bool val)
        {
            // TODO: Remove the main if statement?
            if (state != WriterState.ExpectingArrayElement)
            {
                writer.WriteString(val.ToString());
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteStartElement(ArrayElemStr);
                writer.WriteString(val.ToString());
                writer.WriteEndElement();
            }
            pathBuilder.OnValue();
        }

        private void StructuredVal(string valStr)
        {
            if (state != WriterState.ExpectingArrayElement)
            {
                writer.WriteString(valStr);
                writer.WriteEndElement();
            }
            else if (arrayTypeStack.Peek() == TrwValueType.Undefined)
            {
                writer.WriteStartElement(ArrayElemStr);
                writer.WriteString(valStr);
                writer.WriteEndElement();
            }
            else
            {
                if (structuredArrayBuilder.Length > 1)
                    structuredArrayBuilder.Append(", ");
                structuredArrayBuilder.Append(valStr);
            }
            pathBuilder.OnValue();
        }

        public void SInt8(sbyte val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void UInt8(byte val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void SInt16(short val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void UInt16(ushort val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void SInt32(int val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void UInt32(uint val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void SInt64(long val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void UInt64(ulong val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void Float32(float val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        public void Float64(double val) => StructuredVal(val.ToString(CultureInfo.InvariantCulture));
        #endregion
    }
}