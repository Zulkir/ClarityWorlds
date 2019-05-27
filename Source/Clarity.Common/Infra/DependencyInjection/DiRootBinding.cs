using System;
using System.Collections.Generic;
using System.Linq;

namespace Clarity.Common.Infra.DependencyInjection
{
    public class DiRootBinding : IDiRootBinding
    {
        private readonly List<DiRootBindingItem> items;
        private readonly Type abstractType;
        private int currentIndex;
        private readonly DiConstructorBinding concreteTypeBinding;

        public DiRootBinding(Type abstractType)
        {
            this.abstractType = abstractType;
            items = new List<DiRootBindingItem>();
            if (!abstractType.IsAbstract)
                if (!DiConstructorBinding.TryCreate(abstractType, out concreteTypeBinding))
                    concreteTypeBinding = null;
        }

        public Type AbstractType => abstractType;
        public IReadOnlyList<IDiRootBindingItem> Items => items;

        public object BuildSingle(IDiContainer di, DiBuildInstanceType buildInstanceType)
        {
            if (abstractType.ContainsGenericParameters)
                throw new InvalidOperationException($"Trying to obtain an instance of a non-complete generic type '{abstractType}'.");

            foreach (var item in items)
                if (item.Condition(di))
                    return item.Binding.BuildInstance(di, Type.EmptyTypes, buildInstanceType);

            if (abstractType.IsGenericType)
            {
                var genericItems = GetGenericItems(di, DiRootBindingType.Single);
                foreach (var item in genericItems)
                    if (item.Condition(di))
                        return item.Binding.BuildInstance(di, abstractType.GetGenericArguments(), buildInstanceType);
            }

            if (concreteTypeBinding != null)
                return concreteTypeBinding.BuildInstance(di, abstractType.GetGenericArguments(), buildInstanceType);

            throw new KeyNotFoundException($"No valid bindings found for '{abstractType.FullName}' implementations.");
        }

        public Array BuildMulti(IDiContainer di, DiBuildInstanceType buildInstanceType)
        {
            if (abstractType.ContainsGenericParameters)
                throw new InvalidOperationException($"Trying to obtain an instance of a non-complete generic type '{abstractType}'.");

            var ownResults = items.Where(x => x.Condition(di)).Select(x => x.Binding.BuildInstance(di, Type.EmptyTypes, buildInstanceType));
            var genericResults = GetGenericItems(di, DiRootBindingType.Multi).Select(x => x.Binding.BuildInstance(di, abstractType.GetGenericArguments(), buildInstanceType));

            var objArray = ownResults.Concat(genericResults).ToArray();
            var typedArray = Array.CreateInstance(abstractType, objArray.Length);
            Array.Copy(objArray, typedArray, objArray.Length);
            return typedArray;
        }

        public void AddNewCase(IDiBinding binding)
        {
            var newCase = new DiRootBindingItem(x => true, binding);
            if (currentIndex == items.Count)
                items.Add(newCase);
            else
                items.Insert(currentIndex, newCase);
        }

        public void SetConditionForCurrentCase(Func<IDiContainer, bool> condition)
        {
            items[currentIndex].Condition = condition;
        }

        public void IncCurrent() { currentIndex++; }
        public void MoveToHead() { currentIndex = 0; }
        public void MoveToTail() { currentIndex = items.Count; }

        private IEnumerable<IDiRootBindingItem> GetGenericItems(IDiContainer di, DiRootBindingType rootBindingType)
        {
            return abstractType.IsGenericType
                ? di.GetRootBinding(abstractType.GetGenericTypeDefinition(), rootBindingType).Items
                : Enumerable.Empty<IDiRootBindingItem>();
        }
    }
}