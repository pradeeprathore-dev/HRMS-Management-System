using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class TransportRoute : BaseEntity
    {
        public string RouteName { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
    }
}
