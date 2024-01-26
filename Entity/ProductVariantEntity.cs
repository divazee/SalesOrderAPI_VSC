public class ProductVariantEntity
{
    public int Id { get; set; }

    public string? ProductCode { get; set; }

    public int? ColorId { get; set; }

    public int? SizeId { get; set; }

    public string? Remarks { get; set; }

    public decimal? Price { get; set; }

    public bool? IsActive { get; set; }
}