namespace HRMSAPI.DTOs
{
    public class PaginationResponseDto<T>
    {
        public List<T> Data { get; set; } = new();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPrevious =>
            PageNumber > 1;

        public bool HasNext =>
            PageNumber < TotalPages;
    }
}