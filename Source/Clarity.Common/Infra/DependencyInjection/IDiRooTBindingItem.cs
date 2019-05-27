using System;

namespace Clarity.Common.Infra.DependencyInjection
{
    public interface IDiRootBindingItem
    {
        Func<IDiContainer, bool> Condition { get; }
        IDiBinding Binding { get; }
    }
}