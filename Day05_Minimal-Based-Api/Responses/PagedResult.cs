namespace Day04_Controller_Based.Responses
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        private PagedResult() { }
        public static PagedResult<T> Create(IEnumerable<T> items, int count, int page, int pageSize)
        {
          

            return new PagedResult<T>
            {
                Items = items,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = count,
            };
        }
    }


}
