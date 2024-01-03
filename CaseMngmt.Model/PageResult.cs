using Microsoft.EntityFrameworkCore;

namespace CaseMngmt.Models
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
        }
        public PagedResult(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            Items = items;
        }

        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; }

        public static async Task<PagedResult<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }

        public static async Task<PagedResult<T>> CreateAsync(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
