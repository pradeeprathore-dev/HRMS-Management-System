using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HRMSWEB.Models
{
    public class CreateEmployeeOnboardingViewModel
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Required]
        [EmailAddress]
        public string OfficialEmail { get; set; }

        [Required]
        public string SeatNumber { get; set; }

        public bool ManagerAssigned { get; set; }

        public bool IdCardGenerated { get; set; }

        public bool IsCompleted { get; set; }

        public List<SelectListItem> Employees { get; set; } = new();
    }
}