namespace HRMSWEB.DTOs
{
    public class CreateLeaveRequestDto
    {
        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; }
    }
}
