using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class HRActivity : BaseEntity
    {
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
