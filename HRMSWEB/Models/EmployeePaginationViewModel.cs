namespace HRMSWEB.Models
{
    public class EmployeePaginationViewModel
    {
        public List<EmployeeViewModel> Data { get; set; } = new();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public bool HasPrevious { get; set; }

        public bool HasNext { get; set; }
    }
}