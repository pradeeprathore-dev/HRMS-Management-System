using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class EmployeeDocument
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string DocumentType { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string FileExtension { get; set; }

        public long FileSize { get; set; }

        public bool IsVerified { get; set; }

        public string? Remarks { get; set; }

        public DateTime UploadedDate { get; set; }

        public Employee Employee { get; set; }
    }
}
