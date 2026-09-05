using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickCart.Ordering.Domain.Orders;

namespace QuickCart.Ordering.Infrastructure.Persistence.Configurations;

internal sealed class OrderConfiguration
    : IEntityTypeConfiguration<Order>
{
    public void Configure(
        EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(x => x.PlacedAtUtc)
            .HasColumnName("placed_at_utc");

        builder.OwnsOne(
            x => x.ShippingAddress,
            address =>
            {
                address.Property(x => x.City)
                    .HasColumnName("shipping_city")
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(x => x.District)
                    .HasColumnName("shipping_district")
                    .HasMaxLength(100)
                    .IsRequired();

                address.Property(x => x.Line)
                    .HasColumnName("shipping_address_line")
                    .HasMaxLength(500)
                    .IsRequired();

                address.Property(x => x.PostalCode)
                    .HasColumnName("shipping_postal_code")
                    .HasMaxLength(20);
            });

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(
                PropertyAccessMode.Field);

        builder.Ignore(x => x.Total);

        builder.Ignore(x => x.DomainEvents);

        builder.HasIndex(
                x => new
                {
                    x.CustomerId,
                    x.CreatedAtUtc
                })
            .HasDatabaseName(
                "ix_orders_customer_created_at")
            .IsDescending(false, true);
    }
}