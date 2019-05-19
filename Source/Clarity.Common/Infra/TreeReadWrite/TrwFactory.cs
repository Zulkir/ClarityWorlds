using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Clarity.Common.Infra.TreeReadWrite
{
    public class TrwFactory : ITrwFactory
    {
        private readonly Dictionary<string, ITrwFormat> formats;

        public TrwFactory(IReadOnlyList<ITrwFormat> formats)
        {
            this.formats = formats.ToDictionary(x => x.Name, x => x);
        }

        public ITrwReader Reader(Stream stream, string format) => 
            formats[format].CreateReader(stream);

        public ITrwWriter Writer(Stream stream, string format) =>
            formats[format].CreateWriter(stream);
    }
}