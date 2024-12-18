using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Review
{
	public Guid Id { get; set; }

	public int? Rating { get; set; }

	public string? Text { get; set; }

	public virtual Reservation IdNavigation { get; set; } = null!;

	public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
