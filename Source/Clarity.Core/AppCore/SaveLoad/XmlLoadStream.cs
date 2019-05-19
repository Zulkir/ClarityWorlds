using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class XmlLoadStream : ITreeLoadStream
    {
        private readonly XmlReader reader;
        private readonly Dictionary<string, Type> aliasedTypes;
        private int upMovesToCurrent = 0;
        private string elemName;
        private string attrName;
        private bool isAtAttr;
        private string value;
        private bool isAtTheEnd;

        public string RawValue => value;
        public bool IsEndOfChildren => upMovesToCurrent > 0;
        private int CompensatedDepth => reader.Depth;

        public XmlLoadStream(Stream stream, Dictionary<string, Type> dict)
        {
            aliasedTypes = dict;
            var settings = new XmlReaderSettings();
            reader = XmlReader.Create(stream, settings);
            ReadNextValue();
            ReadNextValue();
            ReadNextValue();
            ReadNextValue();
            ReadNextValue();
        }

        public void Dispose() => reader.Dispose();
        
        private bool ReadNextValue()
        {
            while (reader.MoveToNextAttribute() || reader.Read())
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        attrName = reader.Name;
                        value = reader.Value;
                        isAtAttr = true;
                        return true;
                    case XmlNodeType.Element:
                        elemName = reader.Name;
                        isAtAttr = false;
                        return true;
                    case XmlNodeType.Text:
                        value = reader.Value;
                        isAtAttr = false;
                        return true;
                    default:
                        break;
                }
            if (!isAtTheEnd)
            {
                isAtTheEnd = true;
                return true;
            }
            return false;
        }

        private void CheckNotEnd()
        {
            if (IsEndOfChildren)
                throw new InvalidOperationException("Trying to get a value of an EndOfChildren tree node.");
        }
        
        public void MoveDown()
        {
            if (IsEndOfChildren)
                throw new InvalidOperationException("Trying to move down into a virtual (EndOfChildren) tree node.");
            var depth = CompensatedDepth;
            if (!ReadNextValue())
                throw new Exception("Unexpected end of XML.");
            if (CompensatedDepth == depth + 1)
                return;
            else if (CompensatedDepth == depth)
                upMovesToCurrent = 1;
            else
                throw new Exception($"Unexpected XML node depth difference on MoveDown: {CompensatedDepth - depth}.");
        }

        public void MoveNext()
        {
            if (IsEndOfChildren)
                throw new InvalidOperationException("Trying to move next past an EndOfChildren tree node.");
            var depth = CompensatedDepth;
            if (!ReadNextValue())
                throw new Exception("Unexpected end of XML.");
            if (CompensatedDepth == depth)
                return;
            else if (CompensatedDepth < depth)
                upMovesToCurrent = depth - CompensatedDepth;
            else
                throw new Exception($"Unexpected XML node depth difference on MoveNext: {CompensatedDepth - depth}.");
        }

        public void MoveUp()
        {
            if (upMovesToCurrent == 0)
                throw new InvalidOperationException("MoveUp is only allowed on an EndOfChildren tree node.");
            upMovesToCurrent--;
        }

        private T MoveNextAndReturn<T>(T result)
        {
            MoveNext();
            return result;
        }

        private bool IsAttr(string name)
        {
            return isAtAttr && attrName == name;
        }

        public string Name
        {
            get
            {
                if (IsEndOfChildren)
                    throw new InvalidOperationException("Trying to get a name of an EndOfChildren tree node.");
                return isAtAttr ? attrName : elemName;
            }
        }

        public string LoadString()
        {
            CheckNotEnd();
            if (IsAttr("null"))
                return MoveNextAndReturn<string>(null);
            if (IsAttr("empty"))
                return MoveNextAndReturn("");
            return MoveNextAndReturn(value);
        }

        public Type LoadType()
        {
            CheckNotEnd();
            if (value == "null")
                return MoveNextAndReturn<Type>(null);
            return MoveNextAndReturn(aliasedTypes[value]);
        }

        public bool LoadBool()
        {
            CheckNotEnd();
            var result = bool.Parse(value);
            MoveNext();
            return result;
        }

        public int LoadInt()
        {
            CheckNotEnd();
            var result = int.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
            MoveNext();
            return result;
        }

        public float LoadFloat()
        {
            CheckNotEnd();
            var result = float.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture);
            MoveNext();
            return result;
        }

        public float[] LoadFloatArray()
        {
            CheckNotEnd();
            if (value == "null")
                return MoveNextAndReturn<float[]>(null);
            if (value == "empty")
                return MoveNextAndReturn(new float[0]);
            var array = value.Split(new[] {", "}, StringSplitOptions.RemoveEmptyEntries);
            var result = array.Select(x => float.Parse(x, NumberStyles.Any, CultureInfo.InvariantCulture)).ToArray();
            MoveNext();
            return result;
        }

        public void Skip()
        {
            CheckNotEnd();
            reader.Skip();
            var depth = CompensatedDepth;
            ReadNextValue();
            upMovesToCurrent = depth - CompensatedDepth;
        }
    }
}