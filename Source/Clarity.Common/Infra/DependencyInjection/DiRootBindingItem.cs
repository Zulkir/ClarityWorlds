using System;

namespace Clarity.Common.Infra.DependencyInjection
{
    public class DiRootBindingItem : IDiRootBindingItem
    {
        public Func<IDiContainer, bool> Condition { get; set; }
        public IDiBinding Binding { get; private set; }

        public DiRootBindingItem(Func<IDiContainer, bool> condition, IDiBinding binding)
        {
            Condition = condition;
            Binding = binding;
        }
    }
}