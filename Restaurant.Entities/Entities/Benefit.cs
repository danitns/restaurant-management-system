using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Benefit
{
    public Guid Id { get; set; }

    public Guid? RestaurantId { get; set; }

    public string Name { get; set; } = null!;

    public string Details { get; set; } = null!;

    public int Value { get; set; }

    public virtual Restaurant? Restaurant { get; set; }

    public virtual ICollection<UserBenefit> UserBenefits { get; set; } = new List<UserBenefit>();
}
