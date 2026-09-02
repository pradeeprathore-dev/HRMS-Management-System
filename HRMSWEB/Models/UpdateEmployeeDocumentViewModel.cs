using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWEB.Models
{
    public class UpdateEmployeeDocumentViewModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string DocumentType { get; set; }

        public IFormFile? File { get; set; }

        public bool IsVerified { get; set; }

        public string? Remarks { get; set; }

        public List<SelectListItem> Employees { get; set; }
            = new List<SelectListItem>();
    }
}