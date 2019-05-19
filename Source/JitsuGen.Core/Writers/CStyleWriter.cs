using System;
using System.Text;

namespace JitsuGen.Core.Writers
{
    public class CStyleWriter
    {
        private class CurlyCloser : IDisposable
        {
            private readonly CStyleWriter writer;

            public CurlyCloser(CStyleWriter writer)
            {
                this.writer = writer;
            }

            public void Dispose()
            {
                writer.CloseCurly();
            }
        }

        public StringBuilder Builder { get; }
        public int CurrentIndent { get; private set; }

        public string IndentString { get; set; } = "    ";

        private readonly CurlyCloser curlyCloser;

        public CStyleWriter() : this(new StringBuilder())
        { }

        public CStyleWriter(StringBuilder builder)
        {
            Builder = builder;
            curlyCloser = new CurlyCloser(this);
        }

        public void Write(string str)
        {
            Builder.Append(str);
        }

        public void WriteLine() => Builder.AppendLine();

        public void WriteLine(string str)
        {
            WriteIndent();
            Builder.AppendLine(str);
        }

        public void WriteIndent()
        {
            for (int i = 0; i < CurrentIndent; i++)
                Builder.Append(IndentString);
        }

        public void Tab() => CurrentIndent++;
        public void Untab() => CurrentIndent--;

        public IDisposable Curly()
        {
            OpenCurly();
            return curlyCloser;
        }

        public void OpenCurly()
        {
            WriteLine("{");
            Tab();
        }

        public void CloseCurly()
        {
            Untab();
            WriteLine("}");
        }
    }
}