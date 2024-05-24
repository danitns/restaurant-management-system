using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Restaurant
{
    public Guid Id { get; set; }

    public byte[]? Picture { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int RestaurantTypeId { get; set; }

    public int CityId { get; set; }

    public Guid UserId { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<RestaurantSchedule> RestaurantSchedules { get; set; } = new List<RestaurantSchedule>();

    public virtual RestaurantType RestaurantType { get; set; } = null!;

    public virtual ICollection<Table> Tables { get; set; } = new List<Table>();

    public virtual User User { get; set; } = null!;
}
