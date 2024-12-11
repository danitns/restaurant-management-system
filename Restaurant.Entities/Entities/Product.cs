using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public byte[]? Picture { get; set; }

    public string Name { get; set; } = null!;

    public int SubcategoryId { get; set; }

    public Guid RestaurantId { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual Subcategory Subcategory { get; set; } = null!;
}
