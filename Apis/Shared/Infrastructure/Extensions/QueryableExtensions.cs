using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<TEntity> SortBy<TEntity>(
            this IQueryable<TEntity> source,
            string fieldName,
            bool ascending
        )
            where TEntity : class
        {
            if (ascending)
                return source.OrderBy(fieldName);

            return source.OrderByDescending(fieldName);
        }

        #region Private expression tree helpers

        private static LambdaExpression GenerateSelector<TEntity>(
            string propertyName,
            out Type resultType
        )
            where TEntity : class
        {
            // Create a parameter to pass into the Lambda expression (Entity => Entity.OrderByField).
            var parameter = Expression.Parameter(typeof(TEntity), "Entity");
            //  create the selector part, but support child properties
            PropertyInfo property;
            Expression propertyAccess;
            if (propertyName.Contains('.'))
            {
                // support to be sorted on child fields.
                string[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(
                    childProperties[0],
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                );
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
                for (int i = 1; i < childProperties.Length; i++)
                {
                    property = property.PropertyType.GetProperty(
                        childProperties[i],
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                    );
                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = typeof(TEntity).GetProperty(
                    propertyName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                );
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }
            resultType = property.PropertyType;
            // Create the order by expression.
            return Expression.Lambda(propertyAccess, parameter);
        }

        private static MethodCallExpression GenerateMethodCall<TEntity>(
            IQueryable<TEntity> source,
            string methodName,
            string fieldName
        )
            where TEntity : class
        {
            Type type = typeof(TEntity);
            Type selectorResultType;
            LambdaExpression selector = GenerateSelector<TEntity>(
                fieldName,
                out selectorResultType
            );
            MethodCallExpression resultExp = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { type, selectorResultType },
                source.Expression,
                Expression.Quote(selector)
            );
            return resultExp;
        }

        #endregion Private expression tree helpers

        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(
            this IQueryable<TEntity> source,
            string fieldName
        )
            where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "OrderBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(
            this IQueryable<TEntity> source,
            string fieldName
        )
            where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(
                source,
                "OrderByDescending",
                fieldName
            );
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(
            this IOrderedQueryable<TEntity> source,
            string fieldName
        )
            where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(source, "ThenBy", fieldName);
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> ThenByDescending<TEntity>(
            this IOrderedQueryable<TEntity> source,
            string fieldName
        )
            where TEntity : class
        {
            MethodCallExpression resultExp = GenerateMethodCall(
                source,
                "ThenByDescending",
                fieldName
            );
            return source.Provider.CreateQuery<TEntity>(resultExp) as IOrderedQueryable<TEntity>;
        }

        public static IOrderedQueryable<TEntity> OrderUsingSortExpression<TEntity>(
            this IQueryable<TEntity> source,
            string sortExpression
        )
            where TEntity : class
        {
            string[] orderFields = sortExpression.Split(',');
            IOrderedQueryable<TEntity> result = null;
            for (
                int currentFieldIndex = 0;
                currentFieldIndex < orderFields.Length;
                currentFieldIndex++
            )
            {
                string[] expressionPart = orderFields[currentFieldIndex].Trim().Split(' ');
                string sortField = expressionPart[0];
                bool sortDescending =
                    expressionPart.Length == 2
                    && expressionPart[1].Equals("DESC", StringComparison.OrdinalIgnoreCase);
                if (sortDescending)
                {
                    result =
                        currentFieldIndex == 0
                            ? source.OrderByDescending(sortField)
                            : result.ThenByDescending(sortField);
                }
                else
                {
                    result =
                        currentFieldIndex == 0
                            ? source.OrderBy(sortField)
                            : result.ThenBy(sortField);
                }
            }
            return result;
        }

        public static IQueryable<T> SearchBy<T>(this IQueryable<T> source, string searchValue)
        {
            return source.AsQueryable().Where(GetExpression<T>(searchValue));
        }

        private static Expression<Func<T, bool>> GetExpression<T>(string searchValue)
        {
            var type = typeof(T);
            var properties = type.GetProperties();

            var parameter = Expression.Parameter(type, "x");
            Expression body = Expression.Constant(false);

            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var searchValueExpression = Expression.Constant(searchValue, typeof(string));

            foreach (var property in properties)
            {
                // Verifica se a propriedade pode ser convertida para string
                if (property.PropertyType == typeof(string) || property.PropertyType.IsValueType)
                {
                    var propertyExpression = Expression.Property(parameter, property);
                    Expression toStringExpression =
                        property.PropertyType == typeof(string)
                            ? propertyExpression
                            : Expression.Call(
                                propertyExpression,
                                property.PropertyType.GetMethod("ToString", Type.EmptyTypes)
                            );

                    var containsExpression = Expression.Call(
                        toStringExpression,
                        containsMethod,
                        searchValueExpression
                    );

                    body = Expression.OrElse(body, containsExpression);
                }
            }

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
