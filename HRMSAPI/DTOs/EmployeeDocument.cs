using Microsoft.AspNetCore.Http;

namespace HRMSAPI.DTOs.EmployeeDocument
{
    public class EmployeeDocumentCreateDto
    {
        public int EmployeeId { get; set; }

        public string DocumentType { get; set; }

        public IFormFile File { get; set; }

        public string? Remarks { get; set; }
    }
}