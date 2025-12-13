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

    public static string GetNestedPropertyPath<TDataModel, TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda)
    {
        if (propertyLambda.Body is not MemberExpression member)
        {
            return ((propertyLambda.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
        }

        var path = member.Member.Name;
        while (member.Expression is MemberExpression innerMember)
        {
            path = innerMember.Member.Name + "." + path;
            member = innerMember;
        }

        return path;
    }

    public static string GetNestedPropertyPathObj<TDataModel>(Expression<Func<TDataModel, object>> propertyLambda)
    {
        MemberExpression member = propertyLambda.Body as MemberExpression;
        if (member == null)
        {
            if (propertyLambda.Body is UnaryExpression unary && unary.Operand is MemberExpression unaryMember)
            {
                member = unaryMember;
            }
            else
            {
                throw new ArgumentException("Invalid expression", nameof(propertyLambda));
            }
        }

        var pathParts = new System.Collections.Generic.List<string>();
        while (member != null)
        {
            pathParts.Insert(0, member.Member.Name);
            member = member.Expression as MemberExpression;
        }

        return string.Join(".", pathParts);
    }

    public static bool IsRootProperty<TDataModel, TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda) where TProperty : class
    {
        return propertyLambda.Body is UnaryExpression;
    }
}
