namespace DelMe.NBench.Demo.PerfAssert.Library.Utils.Expressions
{
    using System;
    using System.Linq.Expressions;

    namespace Expressions
    {
        internal static class ExpressionUtils
        {
            internal static string GetPerfBenchmarkName<T>(Expression<Action<T>> fromAction)
            {
                var methodCallExp = (MethodCallExpression)fromAction.Body;
                var target = methodCallExp.Object;
                string methodName = methodCallExp.Method.Name;
                return $"{target.Type.FullName}+{methodName}";
            }
        }
    }
}