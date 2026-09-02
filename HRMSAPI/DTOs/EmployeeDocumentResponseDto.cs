namespace HRMSAPI.DTOs.EmployeeDocument
{
    public class EmployeeDocumentResponseDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string DocumentType { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileExtension { get; set; }

        public long FileSize { get; set; }

        public bool IsVerified { get; set; }

        public string? Remarks { get; set; }

        public DateTime UploadedDate { get; set; }
    }
}