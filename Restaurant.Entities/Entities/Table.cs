using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Table
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int Seats { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
