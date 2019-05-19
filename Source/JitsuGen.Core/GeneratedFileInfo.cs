using System;
using System.Collections.Generic;

namespace JitsuGen.Core
{
    public struct GeneratedFileInfo
    {
        public string Path;
        public string Content;
        public IEnumerable<Type> TypesUsed;
        public IEnumerable<string> SymbolsUsed;
        public Type Template;
        public string ImplementationFullName;
    }
}