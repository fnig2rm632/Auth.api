using Auth.Domain.Common;
using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data.Context;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<UserIdentity> UserIdentities { get; set; }
    public DbSet<OAuthToken> OAuthTokens { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Session> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }
}