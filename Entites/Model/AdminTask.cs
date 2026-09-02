using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class AdminTask : BaseEntity
    {
        public string TaskName { get; set; }
        public string Status { get; set; }
    }
}
