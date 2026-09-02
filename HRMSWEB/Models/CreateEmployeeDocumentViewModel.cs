using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSWEB.Models
{
    public class CreateEmployeeDocumentViewModel
    {
        public int EmployeeId { get; set; }

        public string DocumentType { get; set; }

        public IFormFile File { get; set; }

        public string? Remarks { get; set; }

        public List<SelectListItem> Employees { get; set; }
            = new List<SelectListItem>();
    }
}