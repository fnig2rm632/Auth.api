using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations;

internal sealed class OAuthTokenConfiguration : IEntityTypeConfiguration<OAuthToken>
{
    public void Configure(EntityTypeBuilder<OAuthToken> builder)
    {
        builder.HasKey(e => e.Id).HasName("oauth_token_pk");
        builder.ToTable("oauth_token", "auth");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        builder.Property(e => e.AccessToken)
            .IsRequired()
            .HasColumnName("access_token");
        builder.Property(e => e.RefreshToken)
            .HasColumnName("refresh_token");
        builder.Property(e => e.ExpiresAt)
            .HasColumnName("expires_at");
            
        builder.Property(e => e.UserIdentityId)
            .HasColumnName("user_identity_id");

        builder.HasOne(d => d.UserIdentity)
            .WithMany(p => p.OAuthTokens)
            .HasForeignKey(d => d.UserIdentityId)
            .HasConstraintName("fk_external_token_identity");

    }
}