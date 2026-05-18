using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations;

internal sealed class UserIdentityConfiguration : IEntityTypeConfiguration<UserIdentity>
{
    public void Configure(EntityTypeBuilder<UserIdentity> builder)
    {
        builder.HasKey(e => e.Id).HasName("user_identity_pk");
        builder.ToTable("user_identity", "auth");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        builder.Property(e => e.ProviderUserId)
            .IsRequired()
            .HasColumnName("provider_user_id");
            
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");
        builder.Property(e => e.ProviderId)
            .HasColumnName("provider_id");

        builder.HasIndex(e => new { e.ProviderId, e.ProviderUserId })
            .IsUnique()
            .HasDatabaseName("uq_provider_user");
    }
}