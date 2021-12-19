using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Shared.Infrastructure.Collections
{
    public class PaginatedList<T>
    {
        public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            HasPreviousPage = PageIndex > 1;
            HasNextPage = PageIndex < TotalPages;

            Data = new List<T>(items);
        }

        public PaginatedList() { }

        public List<T> Data { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}