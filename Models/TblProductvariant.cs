using System;
using System.Collections.Generic;

namespace SalesOrderAPI.Models;

public partial class TblProductvariant
{
    public int Id { get; set; }

    public string? ProductCode { get; set; }

    public int? ColorId { get; set; }

    public int? SizeId { get; set; }

    public string? Remarks { get; set; }

    public decimal? Price { get; set; }

    public bool? IsActive { get; set; }
}
