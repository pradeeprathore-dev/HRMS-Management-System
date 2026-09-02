using HRMSAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites.Model
{
    public class ITAsset : BaseEntity
    {
        public string AssetName { get; set; }

        public string AssetType { get; set; }

        public string AssetCode { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string SerialNumber { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal PurchasePrice { get; set; }

        // Available, Assigned, Repair, Lost
        public string Status { get; set; }

        public string Remarks { get; set; }

        // FK
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }
    }
}
