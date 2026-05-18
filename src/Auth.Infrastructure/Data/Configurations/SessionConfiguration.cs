using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations;

internal sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(e => e.Id).HasName("session_pk");
        builder.ToTable("session", "auth");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
    
        builder.Property(e => e.RefreshTokenHash)
            .IsRequired()
            .HasColumnName("refresh_token_hash");
    
        builder.Property(e => e.RevokedAt)
            .HasColumnName("revoked_at");
    
        builder.Property(e => e.IpAddress)
            .HasMaxLength(45)
            .HasColumnName("ip_address");
    
        builder.Property(e => e.ExpiresAt)
            .IsRequired()
            .HasColumnName("expires_at");
    
        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
            
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");
    
        builder.Property(e => e.DeviceId)
            .HasColumnName("device_id");

        builder.HasOne(d => d.User)
            .WithMany(p => p.Sessions)
            .HasForeignKey(d => d.UserId)
            .HasPrincipalKey(u => u.Id)
            .HasConstraintName("fk_session_user")
            .IsRequired(false);

        builder.HasOne(d => d.Device)
            .WithMany(p => p.Sessions)
            .HasForeignKey(d => d.DeviceId)
            .HasConstraintName("fk_session_device")
            .IsRequired(false);
        
        builder.Navigation(s => s.User)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
            
        builder.Navigation(s => s.Device)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}