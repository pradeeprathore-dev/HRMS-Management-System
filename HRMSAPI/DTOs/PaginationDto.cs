namespace HRMSAPI.DTOs
{
    public class PaginationDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;
        public string? Search { get; set; }
        public int? DepartmentId { get; set; }
        public string? SortBy { get; set; }

        public string? SortOrder { get; set; }
        public string? SearchText { get; set; }
    }
}
