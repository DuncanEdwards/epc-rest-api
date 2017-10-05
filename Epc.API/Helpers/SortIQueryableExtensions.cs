using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Epc.API.Helpers;

namespace Epc.API.Helpers
{
    public static class SortIQueryableExtensions
    {

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (String.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(',');

            //Iterate though each order by statement (in reverse to preserve order correctly)
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var orderByAsTuple = OrderByHelper.ParseOrderBy(orderByClause);
                var propertyName = orderByAsTuple.Item1;
                var isOrderDescending = orderByAsTuple.Item2;

                var userProperty = source.ElementType.GetProperty(propertyName);
                if (userProperty == null)
                {
                    //Sortign by an invalid field
                    throw new ArgumentNullException("userProperty");
                }
                source = source.OrderBy(propertyName + (isOrderDescending ? " descending" : " ascending"));
            }
            return source;
        }

    }
}
