using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataMigrations.Helpers
{
    static class ExpressionHelper
    {
        public static string GetMemberName<T>(this Expression<Func<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression), "Cannot provide a null expression");

            var body = expression.Body as MemberExpression;

            if (body == null) throw new NotImplementedException(
                    $"Cannot get member name from expression type '{expression.Body.GetType().Name}'");

            return body.Member.Name;
        }
    }
}
