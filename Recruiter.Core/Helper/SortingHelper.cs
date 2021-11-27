using Recruiter.Core.Common.Enums;
using Recruiter.Core.Entities.ViewModel.Requests;
using Recruiter.Core.Entities.ViewModel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Recruiter.Core.Helper
{
    public static class SortingHelper
    {
        public static IOrderedQueryable<TSource> SortBy<TSource, TKey>
            (this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            string direction)
        {
            return direction == SortingDirectionEnum.ASC.ToString() ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }

        public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>
            (this IOrderedQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            string direction)
        {
            return direction == SortingDirectionEnum.ASC.ToString() ? source.ThenBy(keySelector) : source.ThenByDescending(keySelector);
        }

        public static IQueryable<T> SortBy<T>(
            this IQueryable<T> query,
            BaseRequestSorting requestSorting,
            BaseResultData resultData)
        {
            if (requestSorting == null)
            {
                resultData.sorting = null;
                return query;
            }
            else
            {
                // LAMBDA: x => x.[PropertyName]
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression property = Expression.Property(parameter, requestSorting.column);
                var lambda = Expression.Lambda(property, parameter);

                resultData.sorting = new ResultSorting
                {
                    column = requestSorting.column,
                    direction = requestSorting.direction
                };

                // REFLECTION: source.OrderBy(x => x.Property)
                var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderBy" && x.GetParameters().Length == 2);
                if (resultData.sorting.direction == SortingDirectionEnum.DESC.ToString())
                {
                    orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == "OrderByDescending" && x.GetParameters().Length == 2);
                }

                var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(T), property.Type);
                var result = orderByGeneric.Invoke(null, new object[] { query, lambda });

                return (IQueryable<T>)result;

            }
        }
    }
}
