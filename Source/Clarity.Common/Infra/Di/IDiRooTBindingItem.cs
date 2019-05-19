using System;

namespace Clarity.Common.Infra.Di
{
    public interface IDiRootBindingItem
    {
        Func<IDiContainer, bool> Condition { get; }
        IDiBinding Binding { get; }
    }
}