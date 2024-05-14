using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public int RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid PasswordHash { get; set; }

    public string Phone { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();

    public virtual Role Role { get; set; } = null!;
}
