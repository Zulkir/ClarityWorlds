using System.Collections.Generic;

namespace Clarity.App.Transport.Prototype.Queries.Infra
{
    public interface IInfraQueryFunction
    {
        string Name { get; }
        IInfraQueryReturnType ReturnType { get; }
        IReadOnlyList<(IInfraQueryReturnType type, string name)> Parameters { get; }
    }

    public interface IInfraQueryFunction<out TResult> : IInfraQueryFunction
    {
        TResult Call(IInfraQueryExecutionContext context);
    }

    public interface IInfraQueryFunction<out TResult, in TArg0> : IInfraQueryFunction
    {
        TResult Call(IInfraQueryExecutionContext context, TArg0 arg0);
    }

    public interface IInfraQueryFunction<out TResult, in TArg0, in TArg1> : IInfraQueryFunction
    {
        TResult Call(IInfraQueryExecutionContext context, TArg0 arg0, TArg1 arg1);
    }

    public interface IInfraQueryFunction<out TResult, in TArg0, in TArg1, in TArg2> : IInfraQueryFunction
    {
        TResult Call(IInfraQueryExecutionContext context, TArg0 arg0, TArg1 arg1, TArg2 arg2);
    }

    public interface IInfraQueryFunction<out TResult, in TArg0, in TArg1, in TArg2, in TArg3> : IInfraQueryFunction
    {
        TResult Call(IInfraQueryExecutionContext context, TArg0 arg0, TArg1 arg1, TArg2 arg2, TArg3 arg3);
    }
}