using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Training:BaseEntity
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}
