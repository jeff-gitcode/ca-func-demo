using Application.Services;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CosmosDB
{
    public static class FilterExtension
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, Specification<T>? specification) where T : BaseEntity
        {
            if (specification is null) return query;
            return query.Where(specification.Build());
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, Pagination? pagination) where T : BaseEntity
        {
            if (pagination is null) return query;
            return query.Skip((pagination.Page - 1) * pagination.Rows).Take(pagination.Rows);
        }
    }
}
