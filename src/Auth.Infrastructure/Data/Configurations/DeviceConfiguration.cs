using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Data.Configurations;

internal sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(e => e.Id).HasName("device_pk");
        builder.ToTable("device", "auth");

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");
        
        builder.Property(e => e.PlatformId)
            .HasColumnName("platform_id");

        builder.HasOne(d => d.Platform)
            .WithMany(p => p.Devices)
            .HasForeignKey(d => d.PlatformId)
            .HasConstraintName("fk_device_platform")
            .OnDelete(DeleteBehavior.SetNull);
    }
}