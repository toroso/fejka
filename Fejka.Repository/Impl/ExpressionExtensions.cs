using System.Linq.Expressions;
using System.Reflection;

namespace Fejka.Repository.Impl;

internal static class ExpressionExtensions
{
    public static MemberInfo GetMemberInfo(this Expression expression)
    {
        var lambda = (LambdaExpression)expression;
        return lambda.Body switch
        {
            UnaryExpression unaryExpression => ((MemberExpression)unaryExpression.Operand).Member,
            MethodCallExpression methodCallExpression => methodCallExpression.Method,
            _ => ((MemberExpression)lambda.Body).Member
        };
    }
}
