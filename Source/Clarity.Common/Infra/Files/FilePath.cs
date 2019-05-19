using System;
using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Tuples;

namespace Clarity.Common.Infra.Files
{
    public struct FilePath
    {
        private static readonly char[] Separators = {'/', '\\'};

        private readonly string[] components;

        public bool IsEmpty => Count == 0;
        public int Count => components?.Length ?? 0;
        public int UpCount => IsEmpty ? 0 : components.TakeWhile(x => x == "..").Count();

        public FilePath(string path)
        {
            components = NormalizeComponents(path.Split(Separators, StringSplitOptions.RemoveEmptyEntries));
        }

        public FilePath(string[] compoenents)
        {
            components = NormalizeComponents(compoenents);
        }

        public string this[int index] => components[index];
        
        public FilePath Up()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Trying to call Up() on an empty FilePath.");
            if (components.Length == 0)
                return Empty;
            return new FilePath(components.Take(components.Length - 1).ToArray());
        }

        public FilePath CombineWith(string relativePath) =>
            CombineWith(new FilePath(relativePath));

        public FilePath CombineWith(FilePath relativePath)
        {
            if (IsEmpty)
                return relativePath;
            if (relativePath.IsEmpty)
                return this;
            return new FilePath(NormalizeComponents(components.Concat(relativePath.components).ToArray()));
        }

        public FilePath GetRelativePathFrom(string folderPath) =>
            GetRelativePathFrom(new FilePath(folderPath));

        public FilePath GetRelativePathFrom(FilePath folderPath)
        {
            var sameCount = 0;
            while (Count > sameCount && folderPath.Count > sameCount && this[sameCount] == folderPath[sameCount])
                sameCount++;
            return new FilePath(Enumerable.Range(0, folderPath.Count - sameCount).Select(x => "..").Concat(components.Skip(sameCount)).ToArray());
        }

        public bool IsSubpathOf(FilePath other)
        {
            if (IsEmpty)
                return true;
            if (other.Count < Count)
                return false;
            return components.Zip(other.components, Tuples.SameTypePair).All(x => x.First == x.Second);
        }

        public override string ToString()
        {
            return !IsEmpty ? string.Join("/", components) : "";
        }

        private static string[] NormalizeComponents(string[] components)
        {
            while (true)
            {
                var componentsLoc = components;
                var index = Enumerable.Range(0, components.Length)
                    .Skip(1)
                    .Where(x => componentsLoc[x] == ".." && componentsLoc[x - 1] != "..")
                    .FirstOrNull();
                if (!index.HasValue)
                    break;
                var newComponents = new string[components.Length - 2];
                for (int i = 0; i < index.Value - 1; i++)
                    newComponents[i] = components[i];
                for (int i = index.Value + 1; i < components.Length; i++)
                    newComponents[i - 2] = components[i];
                components = newComponents;
            }
            return components;
        }

        public static FilePath Empty => new FilePath();
    }
}