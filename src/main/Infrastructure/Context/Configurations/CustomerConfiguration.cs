using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NttBankApi.Domain.Entities;

namespace NttBankApi.Infrastructure.Context.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    private const int NameMaxLength = 256;
    private const int EmailMaxLength = 254;
    private const int EnumMaxLength = 32;

    public void Configure(
        EntityTypeBuilder<Customer> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.ToTable("customers");

        builder.HasKey(customer => customer.Id);

        builder.Property(customer => customer.Id)
            .ValueGeneratedOnAdd();

        builder.Property(customer => customer.Name)
            .HasMaxLength(NameMaxLength)
            .IsRequired();

        builder.ComplexProperty(
            customer => customer.Email,
            email => email.Property(value => value.Value)
                .HasColumnName("email")
                .HasMaxLength(EmailMaxLength)
                .IsRequired());

        builder.Property(customer => customer.CreatedAt)
            .IsRequired();

        builder.Property(customer => customer.AccountType)
            .HasConversion<string>()
            .HasMaxLength(EnumMaxLength)
            .IsRequired();

        builder.Property(customer => customer.Status)
            .HasConversion<string>()
            .HasMaxLength(EnumMaxLength)
            .IsRequired();
    }
}
