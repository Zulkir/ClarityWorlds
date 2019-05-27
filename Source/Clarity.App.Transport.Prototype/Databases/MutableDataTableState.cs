using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Clarity.Common.CodingUtilities;
using Clarity.Common.Numericals.Algebra;

namespace Clarity.App.Transport.Prototype.Databases
{
    public class MutableDataTableState : IMutableDataTableState
    {
        public IDataTable Table { get; }
        public int RowCount => indicesByKey.Count;

        private readonly IDataTableDataLayout layout;
        private readonly IList[] lists;

        private readonly Dictionary<int, int> indicesByKey;
        private readonly List<(int, int)> indicesByKeyChanges;
        private int maxPrimaryKey;

        public MutableDataTableState(IDataTable table, IDataTableState initialState = null)
        {
            Table = table;
            layout = new DataTableDataLayout(table.Fields);
            lists = DataField.AllTypes.Select(x =>
            {
                if (!layout.Elements.Any(y => y.Type == x))
                    return (IList)null;
                switch (x)
                {
                    case DataFieldType.Int32: return new List<int>();
                    case DataFieldType.Float: return new List<float>();
                    case DataFieldType.Pc64: return new List<DPolyCubic>();
                    case DataFieldType.String: return new List<string>();
                    default: throw new ArgumentOutOfRangeException(nameof(x), x, null);
                }
            }).ToArray();
            indicesByKey = new Dictionary<int, int>();
            indicesByKeyChanges = new List<(int, int)>();
            if (initialState != null)
                AppendExisting(initialState);
        }

        public ICollection<int> Keys => indicesByKey.Keys;

        private IEnumerable<DataFieldType> GetAllUsedTypes() => 
            DataField.AllTypes.Where(x => lists[(int)x] != null);

        private interface IGenericFunctor<in TArgs>
        {
            void Invoke<T>(MutableDataTableState self, DataFieldType type, TArgs rowKey);
        }

        private void InvokeGeneric<TArgs>(IGenericFunctor<TArgs> functor, DataFieldType type, TArgs args)
        {
            switch (type)
            {
                case DataFieldType.Int32: functor.Invoke<int>(this, type, args); return;
                case DataFieldType.Float: functor.Invoke<float>(this, type, args); return;
                case DataFieldType.Pc64: functor.Invoke<DPolyCubic>(this, type, args); return;
                case DataFieldType.String: functor.Invoke<string>(this, type, args); return;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private List<T> GetList<T>(int fieldIndex, out int stride) => GetList<T>(layout.Elements[fieldIndex].Type, out stride);
        private List<T> GetList<T>(DataFieldType type, out int stride)
        {
            stride = layout.GetStrideForType(type);
            return lists[(int)type] as List<T> ?? throw new ArgumentException($"Typying to access type '{type}' as '{typeof(T).Name}'.");
        }

        private IList GetListAbstract(int fieldIndex, out int stride) => GetListAbstract(layout.Elements[fieldIndex].Type, out stride);
        private IList GetListAbstract(DataFieldType type, out int stride)
        {
            stride = layout.GetStrideForType(type);
            return lists[(int)type];
        }

        private static T GetDefaultValue<T>(DataFieldType type)
        {
            switch (type)
            {
                case DataFieldType.String: return (T)("" as object);
                default: return default;
            }
        }

        public bool HasRow(int key) => indicesByKey.ContainsKey(key);

        public T GetValue<T>(int rowKey, int fieldIndex) => GetValueByIndex<T>(indicesByKey[rowKey], fieldIndex);
        public T GetValueByIndex<T>(int rowIndex, int fieldIndex) => 
            GetList<T>(fieldIndex, out var stride)[rowIndex * stride + layout.Elements[fieldIndex].IndexOffset];

        public object GetValueAbstract(int rowKey, int fieldIndex) => GetValueAbstractByIndex(indicesByKey[rowKey], fieldIndex);
        public object GetValueAbstractByIndex(int rowIndex, int fieldIndex) =>
            GetListAbstract(fieldIndex, out var stride)[rowIndex * stride + layout.Elements[fieldIndex].IndexOffset];

        private class InsertNewRowFunctor : IGenericFunctor<int>
        {
            public static InsertNewRowFunctor Singleton { get; } = new InsertNewRowFunctor();
            public void Invoke<T>(MutableDataTableState self, DataFieldType type, int rowIndex) => 
                self.GetList<T>(type, out var stride)?.InsertRange(rowIndex * stride, Enumerable.Repeat(GetDefaultValue<T>(type), stride));
        }

        public void InsertNewRow(int rowKey)
        {
            if (indicesByKey.ContainsKey(rowKey))
                throw new ArgumentException($"A row with the primary key '{rowKey}' already exists.");
            var index = RowCount;
            indicesByKey.Add(rowKey, index);
            CodingHelper.UpdateIfGreater(ref maxPrimaryKey, rowKey);
            foreach (var type in GetAllUsedTypes())
                InvokeGeneric(InsertNewRowFunctor.Singleton, type, index);
            SetValue(rowKey, 0, rowKey);
        }

        private class AddNewRowFunctor : IGenericFunctor<object>
        {
            public static AddNewRowFunctor Singleton { get; } = new AddNewRowFunctor();
            public void Invoke<T>(MutableDataTableState self, DataFieldType type, object rowKey)
            {
                var list = self.GetList<T>(type, out var stride);
                for (var i = 0; i < stride; i++)
                    list.Add(GetDefaultValue<T>(type));
            }
        }

        public void AddNewRow(out int rowKey)
        {
            foreach (var type in GetAllUsedTypes())
                InvokeGeneric(AddNewRowFunctor.Singleton, type, default);
            maxPrimaryKey++;
            rowKey = maxPrimaryKey;
            var index = RowCount;
            indicesByKey.Add(rowKey, index);
        }

        private class DeleteRowFunctor : IGenericFunctor<int>
        {
            public static DeleteRowFunctor Singleton { get; } = new DeleteRowFunctor();
            public void Invoke<T>(MutableDataTableState self, DataFieldType type, int rowIndex)
            {
                var list = self.GetList<T>(type, out var stride);
                list.RemoveRange(rowIndex * stride, stride);
            }
        }

        public void DeleteRow(int rowKey)
        {
            var index = indicesByKey[rowKey];
            foreach (var type in GetAllUsedTypes())
                InvokeGeneric(DeleteRowFunctor.Singleton, type, index);
            var oldIndex = indicesByKey[rowKey];
            indicesByKey.Remove(rowKey);
            foreach (var kvp in indicesByKey)
                if (kvp.Value >= oldIndex)
                    indicesByKeyChanges.Add((kvp.Key, kvp.Value - 1));
            foreach (var kvp in indicesByKeyChanges)
                indicesByKey[kvp.Item1] = kvp.Item2;
            indicesByKeyChanges.Clear();
        }

        public void SetValue<T>(int rowKey, int fieldIndex, T value) => 
            GetList<T>(fieldIndex, out var stride)[indicesByKey[rowKey] * stride + layout.Elements[fieldIndex].IndexOffset] = value;

        public void SetValueAbstract(int rowKey, int fieldIndex, object value) =>
            GetListAbstract(fieldIndex, out var stride)[indicesByKey[rowKey] * stride + layout.Elements[fieldIndex].IndexOffset] = value;

        private class CopyFieldFunctor : IGenericFunctor<(int localRowKey, int externalRowKey, int fieldIndex, IDataTableState otherReader)>
        {
            public static CopyFieldFunctor Singleton { get; } = new CopyFieldFunctor();
            public void Invoke<T>(MutableDataTableState self, DataFieldType type, (int localRowKey, int externalRowKey, int fieldIndex, IDataTableState otherReader) rowKey) =>
                self.SetValue(rowKey.localRowKey, rowKey.fieldIndex, rowKey.otherReader.GetValue<T>(rowKey.externalRowKey, rowKey.fieldIndex));
        }

        public void AppendExisting(IDataTableState data)
        {
            if (data.Table != Table)
                throw new ArgumentException("Trying to append data from a different table.");
            foreach (var key in data.Keys)
            {
                InsertNewRow(key);
                for (var fieldIndex = 0; fieldIndex < layout.Elements.Count; fieldIndex++)
                    InvokeGeneric(CopyFieldFunctor.Singleton, Table.Fields[fieldIndex].Type,
                        (key, key, fieldIndex, data));
            }
        }

        public IMutableDataTableState CloneAsMutable()
        {
            return new MutableDataTableState(Table, this);
        }
    }
}