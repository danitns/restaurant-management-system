using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class RestaurantSchedule
{
    public Guid Id { get; set; }

    public Guid RestaurantId { get; set; }

    public int DayOfWeek { get; set; }

    public TimeOnly OpeningTime { get; set; }

    public TimeOnly ClosingTime { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;
}
