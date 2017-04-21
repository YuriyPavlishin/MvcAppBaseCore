using System.Linq.Expressions;

namespace BaseApp.Data.Infrastructure
{
    internal class ExpressionClosureFactory
    {
        public static MemberExpression GetField<TValue>(TValue value)
        {
            var closure = new ExpressionClosureField<TValue>
            {
                ValueProperty = value
            };

            return Expression.Field(Expression.Constant(closure), "ValueProperty");
        }

        class ExpressionClosureField<T>
        {
            public T ValueProperty;
        }
    }
}
