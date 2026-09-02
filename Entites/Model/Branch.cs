using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Branch :BaseEntity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public string Location { get; set; }
    }
}
