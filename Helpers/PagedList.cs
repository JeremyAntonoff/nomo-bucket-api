using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NomoBucket.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        
        public PagedList(List<T> items, int currentPage, int totalCount, int pageSize)
        {
            this.CurrentPage = currentPage;
            this.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            this.TotalCount = totalCount;
            this.PageSize = pageSize;
            this.AddRange(items);
        }
        public static async Task<PagedList<T>> CreatePagedList(IQueryable<T> ctx, int currentPage, int pageSize)
        {
            var count = await ctx.CountAsync();
            var list = await ctx.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(list, currentPage, count, pageSize);
        }

    }
}