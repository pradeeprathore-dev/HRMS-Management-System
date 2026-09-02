namespace HRMSAPI.DTOs.EmployeeOnboarding
{
    public class EmployeeOnboardingCreateDto
    {
        public int EmployeeId { get; set; }

        public DateTime JoiningDate { get; set; }

        public string OfficialEmail { get; set; }

        public string SeatNumber { get; set; }

        public bool CredentialsCreated { get; set; }

        public bool DocumentsVerified { get; set; }

        public bool LaptopAllocated { get; set; }

        public bool ManagerAssigned { get; set; }

        public bool ProjectAssigned { get; set; }

        public bool IdCardGenerated { get; set; }

        public bool IsCompleted { get; set; }
    }
}