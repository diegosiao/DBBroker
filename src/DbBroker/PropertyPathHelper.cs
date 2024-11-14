using System;
using System.Linq.Expressions;

namespace DbBroker;

internal static class PropertyPathHelper
{
    public static string GetPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
    {
        var member = propertyLambda.Body as MemberExpression;
        if (member == null)
            throw new ArgumentException("Invalid expression", nameof(propertyLambda));

        return member.Member.Name;
    }

    public static string GetNestedPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
    {
        var member = propertyLambda.Body as MemberExpression;
        if (member == null)
            throw new ArgumentException("Invalid expression", nameof(propertyLambda));

        var path = member.Member.Name;
        while (member.Expression is MemberExpression innerMember)
        {
            path = innerMember.Member.Name + "." + path;
            member = innerMember;
        }

        return path;
    }
}
