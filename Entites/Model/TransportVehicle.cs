using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class TransportVehicle: BaseEntity
    {
        public string VehicleNumber { get; set; }

        public int RouteId { get; set; }
        public TransportRoute Route { get; set; }
    }
}
