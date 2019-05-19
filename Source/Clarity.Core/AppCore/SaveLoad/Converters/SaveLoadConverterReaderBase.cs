using System.Collections.Generic;
using System.Dynamic;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad.Converters
{
    public abstract class SaveLoadConverterReaderBase : ISaveLoadConverter, ITrwReader
    {
        private const string Unknown = "UNKNOWN";
        private const string Value = "VALUE";

        private readonly Stack<string> typeNameStack;
        private string typeName;

        protected abstract ITrwReader GetImmediatePrevious();
        protected string LastProperty { get; private set; }

        private ITrwReader Previous => GetImmediatePrevious();

        protected SaveLoadConverterReaderBase()
        {
            typeNameStack = new Stack<string>();
        }

        public void Dispose()
        {
            Previous.Dispose();
        }

        public virtual bool MoveNext()
        {
            if (!Previous.MoveNext())
                return false;

            if (Previous.TokenType == TrwTokenType.PropertyName)
                LastProperty = Previous.ValueAsString;
            if (Previous.TokenType == TrwTokenType.StartObject)
            {
                typeNameStack.Push(LastProperty == "value" ? Value : Unknown);
            }
            if (Previous.TokenType == TrwTokenType.String && LastProperty == "@type")
            {
                typeName = Previous.ValueAsString;
                typeNameStack.Pop();
                typeNameStack.Push(typeName);
            }
            if (Previous.TokenType == TrwTokenType.EndObject)
                typeNameStack.Pop();
            return true;
        }

        public virtual void Skip()
        {
            Previous.Skip();
        }

        protected string GetTypeName()
        {
            if (typeNameStack.Peek() != Value)
                return typeNameStack.Peek();
            typeNameStack.Pop();
            var type = typeNameStack.Peek();
            typeNameStack.Push(Value);
            return type;
        }


        public virtual TrwTokenType TokenType => Previous.TokenType;
        public virtual bool ValueAsBool => Previous.ValueAsBool;
        public virtual int ValueAsSInt32 => Previous.ValueAsSInt32;
        public virtual double ValueAsFloat64 => Previous.ValueAsFloat64;
        public virtual string ValueAsString => Previous.ValueAsString;
        public virtual int LineNumber => Previous.LineNumber;
        public virtual int LinePosition => Previous.LinePosition;
        public virtual string CurrentEntryPath => Previous.CurrentEntryPath;

        public void ChainFrom(ITrwReader reader) { throw new System.NotImplementedException(); }
        public void ChainFrom(ISaveLoadConverter converter) { throw new System.NotImplementedException(); }
        public ITrwReader GetResultAsReader() { throw new System.NotImplementedException(); }
        public ExpandoObject GetResultAsExpando() { throw new System.NotImplementedException(); }
    }
}
