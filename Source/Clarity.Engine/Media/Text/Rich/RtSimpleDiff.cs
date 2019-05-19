namespace Clarity.Engine.Media.Text.Rich
{
    public class RtSimpleDiff
    {
        public int RemoveStart { get; }
        public string RemovedString { get; }
        public int AddStart { get; }
        public string AddedString { get; }

        public RtSimpleDiff(int removeStart, string removedString, int addStart, string addedString)
        {
            RemoveStart = removeStart;
            RemovedString = removedString;
            AddStart = addStart;
            AddedString = addedString;
        }

        public RtSimpleDiff(string a, string b)
        {
            int start = 0;
            int aEnd = a.Length - 1;
            int bEnd = b.Length - 1;

            while (start < a.Length && start < b.Length && a[start] == b[start])
            {
                start++;
            }

            while (start <= aEnd && start <= bEnd && a[aEnd] == b[bEnd])
            {
                aEnd--;
                bEnd--;
            }

            RemoveStart = start;
            RemovedString = a.Substring(start, aEnd - start + 1);
            AddStart = start;
            AddedString = b.Substring(start, bEnd - start + 1);
        }

        public RtSimpleDiff Invert()
        {
            return new RtSimpleDiff(AddStart, AddedString, RemoveStart, RemovedString);
        }
    }
}