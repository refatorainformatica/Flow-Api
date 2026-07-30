using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Domain.Abstractions.Primitives
{
    public abstract class PaginationBase
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }
    }

    public sealed class Pagination<T> : PaginationBase
        where T : class
    {
        public IEnumerable<T> Rows { get; set; }

        public Pagination()
        {
            Rows = [];
        }
    }

    public static class Pagination
    {
        public static Pagination<T> CreatePagination<T>(
            this IQueryable<T> query,
            int offset,
            int limit
        )
            where T : class
        {
            var pagination = new Pagination<T>
            {
                Offset = offset,
                Limit = limit,
                RowCount = query.Count(),
            };

            var pageCount = (double)pagination.RowCount / limit;
            pagination.PageCount = (int)Math.Ceiling(pageCount);
            pagination.Rows = [.. query.Skip(offset).Take(limit)];

            return pagination;
        }
    }
}
