using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class BillProduct
{
    public Guid BillId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Bill Bill { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
