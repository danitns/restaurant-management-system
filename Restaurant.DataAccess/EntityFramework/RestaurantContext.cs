using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Restaurant.Entities;

namespace Restaurant.DataAccess;

public partial class RestaurantContext : DbContext
{
    public RestaurantContext()
    {
    }

    public RestaurantContext(DbContextOptions<RestaurantContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Benefit> Benefits { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Entities.Restaurant> Restaurants { get; set; }

    public virtual DbSet<RestaurantSchedule> RestaurantSchedules { get; set; }

    public virtual DbSet<RestaurantType> RestaurantTypes { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBenefit> UserBenefits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Initial Catalog=Restaurant;Integrated Security=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Benefit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Benefit__3214EC0776924C37");

            entity.ToTable("Benefit");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Details).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Benefits)
                .HasForeignKey(d => d.RestaurantId)
                .HasConstraintName("FK_BENEFIT_RESTAURANT");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07A4A818D5");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "UQ_CATEGORY_NAME").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__City__3214EC074A4F34B6");

            entity.ToTable("City");

            entity.HasIndex(e => e.Name, "UQ_CITY_NAME").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3214EC072C4E4507");

            entity.ToTable("Product");

            entity.HasIndex(e => new { e.Name, e.RestaurantId }, "UQ_PRODUCT_NAME").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Products)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUCT_RESTAURANT");

            entity.HasOne(d => d.Subcategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubcategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRODUCT_SUBCATEGORY");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductR__3214EC07BC7C9FA9");

            entity.ToTable("ProductReview");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Product).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_PRODUCTREVIEW_PRODUCT");

            entity.HasOne(d => d.Review).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.ReviewId)
                .HasConstraintName("FK_PRODUCTREVIEW_REVIEW");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3214EC0789622D52");

            entity.ToTable("Reservation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.Table).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.TableId)
                .HasConstraintName("FK_RESERVATION_TABLE");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RESERVATION_USER");
        });

        modelBuilder.Entity<Entities.Restaurant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC076D95D9F4");

            entity.ToTable("Restaurant");

            entity.HasIndex(e => e.Name, "UQ_RESTAURANT_NAME").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.City).WithMany(p => p.Restaurants)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RESTAURANT_CITY");

            entity.HasOne(d => d.RestaurantType).WithMany(p => p.Restaurants)
                .HasForeignKey(d => d.RestaurantTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RESTAURANT_RESTAURANT_TYPE");

            entity.HasOne(d => d.User).WithMany(p => p.Restaurants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RESTAURANT_USER");
        });

        modelBuilder.Entity<RestaurantSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC078860CE04");

            entity.ToTable("RestaurantSchedule");

            entity.HasIndex(e => new { e.RestaurantId, e.DayOfWeek }, "UQ_RESTAURANTSCHEDULE_UNIQUE").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Restaurant).WithMany(p => p.RestaurantSchedules)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RESTAURANTSCHEDULE_RESTAURANT");
        });

        modelBuilder.Entity<RestaurantType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC070D41BA19");

            entity.ToTable("RestaurantType");

            entity.HasIndex(e => e.Name, "UQ_RESTAURANT_TYPE_NAME").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3214EC07E59C9371");

            entity.ToTable("Review");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Text).HasMaxLength(500);

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Review)
                .HasForeignKey<Review>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_REVIEW_RESERVATION");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC0782531434");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "UQ_ROLE_NAME").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subcateg__3214EC07E602F1D1");

            entity.ToTable("Subcategory");

            entity.HasIndex(e => e.Name, "UQ_SUBCATEGORY_NAME").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Subcategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SUBCATEOGRY_CATEGORY");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Table__3214EC07D1064555");

            entity.ToTable("Table");

            entity.HasIndex(e => new { e.Name, e.RestaurantId }, "UQ_TABLE_NAME").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Restaurant).WithMany(p => p.Tables)
                .HasForeignKey(d => d.RestaurantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TABLE_RESTAURANT");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07D621CD31");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ_USER_EMAIL").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ_USER_PHONE").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE");
        });

        modelBuilder.Entity<UserBenefit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserBene__3214EC07095835BF");

            entity.ToTable("UserBenefit");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.UsedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Benefit).WithMany(p => p.UserBenefits)
                .HasForeignKey(d => d.BenefitId)
                .HasConstraintName("FK_USERBENEFIT_BENEFIT");

            entity.HasOne(d => d.User).WithMany(p => p.UserBenefits)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_USERBENEFIT_USER");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
