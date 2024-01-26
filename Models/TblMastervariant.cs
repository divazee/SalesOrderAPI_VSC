using System;
using System.Collections.Generic;

namespace SalesOrderAPI.Models;

public partial class TblMastervariant
{
    public int Id { get; set; }

    public string? VariantName { get; set; }

    public string? VariantType { get; set; }

    public bool? IsActive { get; set; }
}
