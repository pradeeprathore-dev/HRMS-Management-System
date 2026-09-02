using Microsoft.AspNetCore.Mvc.Rendering;

public class UpdateITAssetViewModel
{
    public int Id { get; set; }
    public string AssetName { get; set; }
    public string AssetType { get; set; }
    public string AssetCode { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PurchasePrice { get; set; }
    public string Status { get; set; }
    public string Remarks { get; set; }
    public int EmployeeId { get; set; }

    public List<SelectListItem> Employees { get; set; }
}