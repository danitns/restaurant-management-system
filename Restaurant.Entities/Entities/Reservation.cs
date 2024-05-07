using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Reservation
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? Phone { get; set; }

    public Guid? TableId { get; set; }

    public DateTime Date { get; set; }

    public virtual Bill? Bill { get; set; }

    public virtual Table? Table { get; set; }

    public virtual User? User { get; set; }
}
