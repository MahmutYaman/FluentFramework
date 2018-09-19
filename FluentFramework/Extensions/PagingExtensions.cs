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
            => items.AsQueryable().ToPagedListAsync(page, page).Result;

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> items, int pageSize, int page)
        {
            var pageCount = (int)Math.Ceiling(await items.CountAsync() / (double)pageSize);
            var skip = pageSize * page;
            return new PagedList<T>
            {
                CurrentPage = page,
                PageCount = pageCount,
                Items = await items.Skip(skip > pageCount ? pageCount : skip).Take(pageSize).ToListAsync()
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
