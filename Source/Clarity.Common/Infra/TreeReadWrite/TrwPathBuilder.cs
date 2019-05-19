using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public class TrwPathBuilder
    {
        private struct PathElem
        {
            public string PropName;
            public int ArrayIndex;
            public bool IsArrayIndex => PropName == null;
            public string PathString => PropName ?? ArrayIndex.ToString();
            public override string ToString() => PathString;
        }

        private readonly Stack<PathElem> pathStack;
        private readonly StringBuilder stringBuilder;
        private PathElem prev;
        private bool popNext;

        public TrwPathBuilder()
        {
            pathStack = new Stack<PathElem>();
            pathStack.Push(new PathElem());
            stringBuilder = new StringBuilder();
            prev = new PathElem{PropName = "LAST"};
        }

        public void OnProperty(string propertyName)
        {
            pathStack.Pop();
            pathStack.Push(new PathElem { PropName = propertyName });
        }

        public void OnStartObject()
        {
            pathStack.Push(new PathElem{PropName = "???"});
        }

        public void OnEndObject()
        {
            pathStack.Pop();
            OnEndValue();
        }

        public void OnStartArray()
        {
            pathStack.Push(new PathElem { ArrayIndex = 0 });
        }

        public void OnEndArray()
        {
            pathStack.Pop();
            OnEndValue();
        }

        public void OnValue()
        {
            OnEndValue();
        }

        private void OnEndValue()
        {
            var prevPath = pathStack.Pop();
            pathStack.Push(prevPath.IsArrayIndex
                ? new PathElem {ArrayIndex = prevPath.ArrayIndex + 1}
                : new PathElem {PropName = "???"});
        }

        public void OnRead(ITrwReader reader)
        {
            if (popNext)
            {
                prev = pathStack.Pop();
                popNext = false;
            }

            switch (reader.TokenType)
            {
                case TrwTokenType.None:
                    break;
                case TrwTokenType.StartObject:
                    OnStartReadValue();
                    prev = new PathElem {PropName = "DUMMY"};
                    break;
                case TrwTokenType.EndObject:
                    popNext = true;
                    break;
                case TrwTokenType.StartArray:
                    OnStartReadValue();
                    prev = new PathElem{ArrayIndex = -1};
                    break;
                case TrwTokenType.EndArray:
                    popNext = true;
                    break;
                case TrwTokenType.PropertyName:
                    pathStack.Push(new PathElem{PropName = reader.ValueAsString});
                    break;
                case TrwTokenType.Null:
                case TrwTokenType.Boolean:
                case TrwTokenType.Integer:
                case TrwTokenType.Float:
                case TrwTokenType.String:
                    OnStartReadValue();
                    popNext = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnSkip(ITrwReader reader)
        {
            pathStack.Pop();
            OnRead(reader);
        }

        private void OnStartReadValue()
        {
            if (prev.IsArrayIndex)
                pathStack.Push(new PathElem{ArrayIndex = prev.ArrayIndex + 1});
        }

        public string BuildPath()
        {
            stringBuilder.Clear();
            var first = true;
            foreach (var elem in pathStack.Reverse().Skip(1))
            {
                if (elem.IsArrayIndex)
                {
                    stringBuilder.Append("[");
                    stringBuilder.Append(elem.ArrayIndex);
                    stringBuilder.Append("]");
                }
                else
                {
                    if (!first)
                        stringBuilder.Append(".");
                    else
                        first = false;
                    stringBuilder.Append(elem.PropName);
                }
            }
            return stringBuilder.ToString();
        }
    }
}