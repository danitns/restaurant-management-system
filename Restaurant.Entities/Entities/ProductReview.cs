using System;
using System.Collections.Generic;

namespace Restaurant.Entities;

public partial class ProductReview
{
    public Guid Id { get; set; }

    public Guid? ReviewId { get; set; }

    public Guid? ProductId { get; set; }

    public byte[]? ImageContent { get; set; }

    public int? Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Review? Review { get; set; }
}
