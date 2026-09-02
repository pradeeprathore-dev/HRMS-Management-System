namespace HRMSWEB.Models
{
    public class PaginationShiftViewModel
    {
        public List<ShiftViewModel> Shifts { get; set; }

        public string SearchText { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }
    }
}
