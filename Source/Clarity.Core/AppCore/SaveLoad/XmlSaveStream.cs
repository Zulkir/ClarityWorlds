using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class XmlSaveStream : ITreeSaveStream
    {
        private readonly Stream stream;
        private readonly StringBuilder textBuffer;
        private readonly XmlWriter writer;
        private readonly Dictionary<string, Type> types;

        private static readonly XmlWriterSettings XmlSettings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace,
            OmitXmlDeclaration = true
        };

        public XmlSaveStream(Stream stream)
        {
            this.stream = stream;
            textBuffer = new StringBuilder();
            writer = XmlWriter.Create(textBuffer, XmlSettings);
            types = new Dictionary<string, Type>();
        }

        public void Dispose()
        {
            writer.Dispose();
            var typesTextBuffer = new StringBuilder();
            using (var typesWriter = XmlWriter.Create(typesTextBuffer, XmlSettings))
            {
                typesWriter.WriteStartElement("TypeAliases");
                foreach (var kvp in types)
                {
                    typesWriter.WriteStartElement("Type");
                    typesWriter.WriteAttributeString("alias", kvp.Key);
                    Debug.Assert(kvp.Value.AssemblyQualifiedName != null, "kvp.Value.AssemblyQualifiedName != null");
                    typesWriter.WriteString(kvp.Value.AssemblyQualifiedName);
                    typesWriter.WriteEndElement();
                }
                typesWriter.WriteEndElement();
            }
            using (var textWriter = new StreamWriter(stream, Encoding.UTF8))
            {
                textWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                textWriter.WriteLine("<SaveFileRoot>");
                textWriter.WriteLine(typesTextBuffer);
                textWriter.WriteLine(textBuffer);
                textWriter.WriteLine("</SaveFileRoot>");
            }
        }

        public void BeginElement(string name)
        {
            writer.WriteStartElement(name);
        }

        public void EndElement()
        {
            writer.WriteEndElement();
        }

        public void AddNullAttribute()
        {
            writer.WriteAttributeString("null", "True");
        }

        public void AddEmptyAttribute()
        {
            writer.WriteAttributeString("empty", "True");
        }

        public void AddAttribute(string name, Type value)
        {
            var alias = GetOrGenerateTypeAlias(value);
            writer.WriteAttributeString(name, alias);
        }

        public void AddAttribute(string name, string value)
        {
            writer.WriteAttributeString(name, value);
        }

        public void FillElementValue(string value)
        {
            if (TryProcessAsNull(value))
                return;
            if (value.Length == 0)
            {
                AddEmptyAttribute();
                return;
            }
            writer.WriteString(value);
        }

        public void FillElementValue(Type value)
        {
            if (TryProcessAsNull(value))
                return;
            var alias = GetOrGenerateTypeAlias(value);
            writer.WriteString(alias);
        }

        public void FillElementValue(bool value)
        {
            writer.WriteString(value.ToString(CultureInfo.InvariantCulture));
        }

        public void FillElementValue(int value)
        {
            writer.WriteString(value.ToString(CultureInfo.InvariantCulture));
        }

        public void FillElementValue(float value)
        {
            writer.WriteString(value.ToString(CultureInfo.InvariantCulture));
        }

        public void FillElementValue(float[] value)
        {
            if (TryProcessAsNull(value))
                return;
            if (value.Length == 0)
            {
                AddEmptyAttribute();
                return;
            }
            var str = string.Join(", ", value.Select(x => x.ToString(CultureInfo.InvariantCulture)));
            writer.WriteString(str);
        }

        private bool TryProcessAsNull(object value)
        {
            if (value != null)
                return false;
            AddNullAttribute();
            return true;
        }

        private string GetOrGenerateTypeAlias(Type type)
        {
            var aliasBase = type.Name;
            var alias = aliasBase;
            var suffix = 1;
            while (types.ContainsKey(alias))
            {
                if (types[alias] == type)
                    return alias;
                alias = aliasBase + suffix.ToString(CultureInfo.InvariantCulture);
                suffix++;
            }
            types.Add(alias, type);
            return alias;
        }
    }
}