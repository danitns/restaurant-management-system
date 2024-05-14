using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Restaurant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int CityId { get; set; }

    public Guid UserId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual User User { get; set; } = null!;
}
