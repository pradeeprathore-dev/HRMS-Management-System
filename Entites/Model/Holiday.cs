using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class Holiday:BaseEntity
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }

        // =========================
        // NEW FIELDS
        // =========================

        public string Description { get; set; }

        public bool IsOptional { get; set; }
    }
}
