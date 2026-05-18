using Auth.Domain.Entities;
using Auth.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;

namespace Auth.Infrastructure.Data.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id).HasName("user_pk");
        builder.ToTable("user", "auth");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        
        builder.Property(e => e.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Conversion(value).Value!
            )
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();
            
        builder.Property(e => e.PasswordHash)
            .HasColumnName("password_hash");
            
        builder.Property(e => e.Login)
            .HasConversion(
                login => login.Value,
                value => Login.Conversion(value).Value!
            )
            .HasColumnName("login")
            .IsRequired();
    }
}