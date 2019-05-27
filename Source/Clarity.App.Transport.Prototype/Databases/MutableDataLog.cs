using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Clarity.Common.CodingUtilities.Collections;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.CodingUtilities.Sugar.Wrappers.Threading;
using JetBrains.Annotations;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class MutableDataLog : IMutableDataLog
    {
        private struct Snapshot
        {
            public readonly int Position;
            [CanBeNull] public readonly string ExternalKey;
            public readonly IMutableDataBaseState State;

            public Snapshot(int position, string externalKey, IMutableDataBaseState state)
            {
                Position = position;
                ExternalKey = externalKey;
                State = state;
            }
        }

        public IReadOnlyList<IDataTable> Tables { get; private set; }
        public int SnapshotInterval { get; private set; }

        private readonly List<IDataLogEntry> entries;
        private readonly List<Snapshot> snapshots;
        private readonly Dictionary<string, double> externalSnapshotTimestamps;
        private readonly ReaderWriterLockSlimSugar readerWriterLock = 
            new ReaderWriterLockSlimSugar(new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion));

        public double StartTimestamp => entries.HasItems() ? entries[0].Timestamp : 0;
        public double EndTimestamp => entries.HasItems() ? entries[entries.Count - 1].Timestamp : 0;

        public MutableDataLog()
        {
            entries = new List<IDataLogEntry>();
            snapshots= new List<Snapshot>();
            externalSnapshotTimestamps = new Dictionary<string, double>();
            Reset(EmptyArrays<IDataTable>.Array);
        }

        #region Reading
        public IDataBaseState GetStateAt(double timestamp)
        {
            using (readerWriterLock.LockRead())
                return GetStateCloneAtInternal(timestamp);
        }

        public IEnumerable<IDataLogEntry> GetEntries(double startTimestamp, double endTimestamp)
        {
            using (readerWriterLock.LockRead())
                return GetEntriesCopyInternal(startTimestamp, endTimestamp);
        }

        public IEnumerable<DataLogReadingState> Read(double startTimestamp, double endTimestamp)
        {
            using (readerWriterLock.LockRead())
            {
                var stateClone = GetStateCloneAtInternal(startTimestamp);
                var entriesCopy = GetEntriesCopyInternal(startTimestamp, endTimestamp);
                return new DataLogEnumerable(stateClone, entriesCopy);
            }
        }

        // Reading Lock Required
        private IMutableDataBaseState GetStateCloneAtInternal(double timestamp)
        {
            var newPos = FirstIndexAfter(timestamp, 0, entries.Count);
            var snapshot = snapshots[GetClosestSnapshotIndex(newPos)];
            // todo: optimize cloning away when possible
            var clone = snapshot.State.CloneAsMutable();
            AdjustState(clone, snapshot.Position, newPos);
            return clone;
        }

        // Reading Lock Required
        private IEnumerable<IDataLogEntry> GetEntriesCopyInternal(double startTimestamp, double endTimestamp)
        {
            var startIndex = FirstIndexAfter(startTimestamp, 0, entries.Count);
            var stopIndex = FirstIndexAfter(endTimestamp, 0, entries.Count);
            var enumerable = Enumerable.Range(startIndex, stopIndex - startIndex).Select(x => entries[x]);
            return enumerable.ToArray();
        }
        #endregion

        #region Modifying
        public void Reset(IReadOnlyList<IDataTable> tables, int snapshotInterval = 1000)
        {
            using (readerWriterLock.LockWrite())
            {
                var externalKeysRemoved = new Queue<string>(snapshots.Select(x => x.ExternalKey).Where(x => x != null));
                entries.Clear();
                snapshots.Clear();
                Tables = tables;
                SnapshotInterval = snapshotInterval;
                var restorationState = new MutableDataBaseState(tables);
                snapshots.Add(new Snapshot(0, null, restorationState.CloneAsMutable()));
                RestoreSnapshotsAndFixUndoablesFrom(0, restorationState, externalKeysRemoved);
            }
        }

        public void ClearFrom(double timestamp)
        {
            using (readerWriterLock.LockWrite())
            {
                var pos = FirstIndexAfter(timestamp, 0, entries.Count);
                CleanSnapshotsTo(pos, out var restorationState, out var externalKeysRemoved);
                entries.RemoveRange(pos, entries.Count - pos);
                RestoreSnapshotsAndFixUndoablesFrom(pos, restorationState, externalKeysRemoved);
            }
        }

        public void AddEntry(IDataLogEntry entry)
        {
            using (readerWriterLock.LockWrite())
            {
                var pos = FirstIndexAfter(entry.Timestamp, 0, entries.Count);
                while (pos < entries.Count && entries[pos].Timestamp <= entry.Timestamp)
                    pos++;
                CleanSnapshotsTo(pos, out var restorationState, out var externalKeysRemoved);
                entries.Insert(pos, entry);
                RestoreSnapshotsAndFixUndoablesFrom(pos, restorationState, externalKeysRemoved);
            }
        }
        
        public void PrepareStateCache(string key, double timestamp)
        {
            using (readerWriterLock.LockWrite())
            {
                var newPos = FirstIndexAfter(timestamp, 0, entries.Count);
                var closestSnapshot = snapshots[GetClosestSnapshotIndex(newPos)];

                IMutableDataBaseState state;
                int oldPos;
                if (externalSnapshotTimestamps.TryGetValue(key, out var oldTimestamp))
                {
                    var index = GetExternalSnapshotIndex(key, oldTimestamp);
                    var oldSnapshot = snapshots[index];
                    if (closestSnapshot.State == oldSnapshot.State ||
                        Math.Abs(index - newPos) < SnapshotInterval / 2)
                    {
                        state = oldSnapshot.State;
                        oldPos = oldSnapshot.Position;
                    }
                    else
                    {
                        state = closestSnapshot.State.CloneAsMutable();
                        oldPos = closestSnapshot.Position;
                    }
                    snapshots.RemoveAt(index);
                }
                else
                {
                    state = closestSnapshot.State.CloneAsMutable();
                    oldPos = closestSnapshot.Position;
                }

                externalSnapshotTimestamps[key] = timestamp;
                AdjustState(state, oldPos, newPos);
                var newIndex = 0;
                while (newIndex < snapshots.Count && snapshots[newIndex].Position > newPos)
                    newIndex++;
                snapshots.Insert(newIndex, new Snapshot(newPos, key, state));
            }
        }

        // Requires Write Lock
        private void CleanSnapshotsTo(int pos, out IMutableDataBaseState restorationState, [CanBeNull] out Queue<string> externalKeysRemoved)
        {
            var closestSnapshotIndex = GetClosestSnapshotIndex(pos);
            var closestSnapshot = snapshots[closestSnapshotIndex];
            var closestSnapshotState = closestSnapshot.State;
            var closestSnapshotPos = closestSnapshot.Position;
            var closestIsAfter = closestSnapshotPos > pos;
            var closestIsLast = closestSnapshotIndex == snapshots.Count - 1;
            var startRemoveSnapshotIndex = closestIsLast || closestIsAfter
                ? closestSnapshotIndex 
                : closestSnapshotIndex + 1;

            externalKeysRemoved = null;
            for (var i = startRemoveSnapshotIndex; i < snapshots.Count; i++)
            {
                var externalKey = snapshots[i].ExternalKey;
                if (externalKey == null) continue;
                if (externalKeysRemoved == null)
                    externalKeysRemoved = new Queue<string>();
                externalKeysRemoved.Enqueue(externalKey);
            }

            snapshots.RemoveRange(startRemoveSnapshotIndex, snapshots.Count - startRemoveSnapshotIndex);

            var keepClosest = closestIsAfter;
            if (keepClosest)
            {
                var currentPos = closestSnapshotPos;
                while (currentPos > pos)
                {
                    entries[currentPos - 1].Undo(closestSnapshotState);
                    currentPos--;
                }
                restorationState = closestSnapshotState;
            }
            else
            {
                var terminalState = closestSnapshotState.CloneAsMutable();
                var currentPos = closestSnapshotPos;
                while (currentPos < pos)
                {
                    entries[currentPos].Apply(terminalState);
                    currentPos++;
                }
                restorationState = terminalState;
            }
        }

        // Requires Write Lock
        private void RestoreSnapshotsAndFixUndoablesFrom(int pos, IMutableDataBaseState restorationState, [CanBeNull] Queue<string> externalKeysRemoved)
        {
            for (var i = pos; i < entries.Count; i++)
            {
                var entry = entries[i];
                entry.MakeUndoable(restorationState);
                if (i % SnapshotInterval == 0)
                    snapshots.Add(new Snapshot(i, null, restorationState.CloneAsMutable()));
                if (externalKeysRemoved?.Count > 0 && entry.Timestamp > externalSnapshotTimestamps[externalKeysRemoved.Peek()])
                    snapshots.Add(new Snapshot(i, externalKeysRemoved.Dequeue(), restorationState.CloneAsMutable()));
                entry.Apply(restorationState);
            }
            snapshots.Add(new Snapshot(entries.Count, null, restorationState));
            if (externalKeysRemoved != null)
                while (externalKeysRemoved.Count > 0)
                    snapshots.Add(new Snapshot(entries.Count, externalKeysRemoved.Dequeue(), restorationState.CloneAsMutable()));
        }
        #endregion

        // Reading Lock Required
        private int FirstIndexAfter(double timestamp, int smallestCandidate, int largestCandidate)
        {
            if (smallestCandidate == largestCandidate)
                return smallestCandidate;
            var middle = (smallestCandidate + largestCandidate) / 2;
            return entries[middle].Timestamp >= timestamp 
                ? FirstIndexAfter(timestamp, smallestCandidate, middle) 
                : FirstIndexAfter(timestamp, middle + 1, largestCandidate);
        }

        // Reading Lock Required
        private int GetClosestSnapshotIndex(int pos)
        {
            // todo: optimize
            return snapshots.Select((s, i) => (s, i)).Minimal(x => Math.Abs(x.s.Position - pos)).i;
        }

        // Reading Lock Required
        private int GetExternalSnapshotIndex(string key, double timestamp)
        {
            // todo: optimize
            return snapshots.IndexOf(x => x.ExternalKey == key) ?? 
                throw new ArgumentException($"Snapshot for an external key '{key}' was not found.");
        }

        // Reading Lock Required
        private void AdjustState(IMutableDataBaseState state, int oldPos, int newPos)
        {
            var pos = oldPos;
            while (pos < newPos)
            {
                entries[pos].Apply(state);
                pos++;
            }
            while (pos > newPos)
            {
                entries[pos - 1].Undo(state);
                pos--;
            }
        }
    }
}