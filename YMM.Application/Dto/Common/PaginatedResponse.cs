using System.Collections.Generic;

namespace YMM.Application.Dto.Common
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        public PaginatedResponse() { }

        public PaginatedResponse(List<T> items, int page, int pageSize, int totalItems)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);
            HasNextPage = page < TotalPages;
            HasPreviousPage = page > 1;
        }
    }
}
