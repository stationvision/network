﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Monitoring.Db.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query.AsSplitQuery();
        }
    }
}
