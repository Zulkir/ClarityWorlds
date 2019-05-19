using System;

namespace Clarity.Common.Infra.Di
{
    public interface IDiMultiToResult
    {
        void If(Func<IDiContainer, bool> condition);
    }
}