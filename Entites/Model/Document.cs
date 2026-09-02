using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Document:BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
