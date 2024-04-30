using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Bill
{
    public Guid Id { get; set; }

    public Guid TableId { get; set; }

    public virtual ICollection<BillProduct> BillProducts { get; set; } = new List<BillProduct>();

    public virtual Reservation IdNavigation { get; set; } = null!;
}
