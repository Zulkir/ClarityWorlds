using System;
using System.Linq.Expressions;

namespace Clarity.Common.CodingUtilities.Sugar.Extensions.Expressions
{
    public static class ExpressionExtensions
    {
        public static string GetPropertyName<TParent, TChild>(this Expression<Func<TParent, TChild>> lambda) => 
            ((MemberExpression)lambda.Body).Member.Name;
    }
}