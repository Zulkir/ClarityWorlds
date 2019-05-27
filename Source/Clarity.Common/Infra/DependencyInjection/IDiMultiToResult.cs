using System;

namespace Clarity.Common.Infra.DependencyInjection
{
    public interface IDiMultiToResult
    {
        void If(Func<IDiContainer, bool> condition);
    }
}