namespace Store.Api.Repository;

class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        //builder.HasIndex(e => e.OrderType).IsUnique();
    }
}

class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        //builder.Property(p => p.RowVersion).IsRowVersion().IsConcurrencyToken();
    }
}