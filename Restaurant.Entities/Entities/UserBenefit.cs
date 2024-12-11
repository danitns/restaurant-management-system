using System;
using System.Collections.Generic;

namespace Restaurant.Entities;
public partial class UserBenefit
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? BenefitId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime? UsedOn { get; set; }

    public virtual Benefit? Benefit { get; set; }

    public virtual User? User { get; set; }
}
