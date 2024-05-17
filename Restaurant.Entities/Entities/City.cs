using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class City
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
}
