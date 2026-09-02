using System.ComponentModel.DataAnnotations;

namespace HRMSAPI.DTOs
{
    public class UpdateITAssetDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string AssetName { get; set; }

        [Required]
        public string AssetType { get; set; }

        [Required]
        public string AssetCode { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public string Status { get; set; }

        public string? Remarks { get; set; }

        [Required]
        public int EmployeeId { get; set; }
    }
}