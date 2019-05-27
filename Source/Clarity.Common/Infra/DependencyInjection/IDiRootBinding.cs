using System;
using System.Collections.Generic;

namespace Clarity.Common.Infra.DependencyInjection
{
    public interface IDiRootBinding
    {
        Type AbstractType { get; }
        IReadOnlyList<IDiRootBindingItem> Items { get; }
        void AddNewCase(IDiBinding binding);
        void SetConditionForCurrentCase(Func<IDiContainer, bool> condition);
        void IncCurrent();
        void MoveToHead();
        void MoveToTail();
        object BuildSingle(IDiContainer di, DiBuildInstanceType buildInstanceType);
        Array BuildMulti(IDiContainer di, DiBuildInstanceType buildInstanceType);
    }
}