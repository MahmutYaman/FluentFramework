using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentFramework.Extensions
{
    public static class PagingExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> items, int pageSize, int page)
            => items.AsQueryable().ToPagedListAsync(pageSize, page).Result;

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> items, int pageSize, int page)
        {
            var total = await items.CountAsync();

            if (total <= pageSize)
                return new PagedList<T>
                {
                    CurrentPage = 1,
                    PageCount = 1,
                    Items = await items.ToListAsync()
                };

            var pageCount = (int)Math.Ceiling(total / (double)pageSize);

            if (page < 1)
                page = 1;
            if (page > pageCount)
                page = pageCount;

            var skip = pageSize * (page - 1);

            return new PagedList<T>
            {
                CurrentPage = page,
                PageCount = pageCount,
                Items = await items.Skip(skip).Take(pageSize).ToListAsync()
            };
        }
    }

    public class PagedList<T>
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
