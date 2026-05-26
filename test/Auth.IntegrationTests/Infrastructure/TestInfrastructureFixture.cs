using System.Diagnostics;
using Auth.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Xunit;

namespace Auth.IntegrationTest.Infrastructure;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestInfrastructureFixture : IAsyncLifetime
{
    private readonly string _dockerComposePath = TakeDockerComposeString();
    private IConnectionMultiplexer? RedisConnection { get; set; }
    private DatabaseContext? DbContext { get; set; }
    
    public IDatabase? RedisDatabase { get; private set; }
    
    private const string RedisConnectionString = "localhost:6380";
    private const string PostgresConnectionString = "Host=localhost;Port=5433;Database=testdb;Username=testuser;Password=testpass";
    
    public async Task InitializeAsync()
    {
        await StartDockerComposeAsync();
        
        await ConnectToRedisAsync();
        await WaitForRedisReadyAsync();
        
        await ConnectToPostgresAsync();
        await WaitForPostgresReadyAsync();
        await InitializeDatabaseAsync();
    }
    
    public async Task DisposeAsync()
    {
        try
        {
            if (RedisConnection != null)
            {
                await RedisConnection.CloseAsync();
                RedisConnection.Dispose();
            }
        
            if (DbContext != null)
            {
                await DbContext.DisposeAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connection: {ex.Message}");
        }
        
        await StopDockerComposeAsync();
    }
    
    public async Task ClearPostgreSqlAsync()
    {
        await DbContext!.Database.ExecuteSqlRawAsync(@"
            DELETE FROM auth.session;
            DELETE FROM auth.device;
            DELETE FROM auth.user;
            DELETE FROM auth.platform;
        ");
    }
    
    public async Task ClearRedisAsync()
    {
        var server = RedisConnection!.GetServer(RedisConnectionString);
        await server.FlushDatabaseAsync();
    }
    
    public DatabaseContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(PostgresConnectionString)
            .Options;
        return new DatabaseContext(options);
    }
    
    private async Task StartDockerComposeAsync()
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                Arguments = $"-f \"{_dockerComposePath}\" up -d",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        await process.WaitForExitAsync();
    }
    
    private async Task StopDockerComposeAsync()
    {
        try
        {
            var stopProcess = new Process
            { 
                StartInfo = new ProcessStartInfo
                {
                    FileName = "docker-compose",
                    Arguments = $"-f \"{_dockerComposePath}\" down -v --remove-orphans",
                    WorkingDirectory = Path.GetDirectoryName(_dockerComposePath),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            
            stopProcess.Start();
            
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await stopProcess.WaitForExitAsync(cts.Token);
            
            if (stopProcess.ExitCode == 0)
            {
                Console.WriteLine("Docker Compose stopped successfully");
            }
            else
            {
                var error = await stopProcess.StandardError.ReadToEndAsync(cts.Token);
                Console.WriteLine($"Docker compose down failed with code {stopProcess.ExitCode}: {error}");
            }
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Docker Compose stop timed out after 10 seconds");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping Docker Compose: {ex.Message}");
        }
    }
    
    private async Task ConnectToRedisAsync()
    {
        var configuration = ConfigurationOptions.Parse(RedisConnectionString);
        configuration.AbortOnConnectFail = false;
        configuration.AllowAdmin = true;
        configuration.ConnectTimeout = 10000;
        configuration.SyncTimeout = 10000;
        
        try
        {
            RedisConnection = await ConnectionMultiplexer.ConnectAsync(configuration);
            RedisDatabase = RedisConnection.GetDatabase();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to connect to Redis: {ex.Message}", ex);
        }
    }

    private async Task WaitForRedisReadyAsync()
    {
        const int maxRetries = 30;
        
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                await RedisDatabase!.PingAsync();
                
                return;
            }
            catch
            {
                if (i == maxRetries - 1)
                {
                    throw new TimeoutException($"Redis did not become ready after {maxRetries} attempts");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }
    }
    
    private Task ConnectToPostgresAsync()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseNpgsql(PostgresConnectionString)
            .Options;
        
        DbContext = new DatabaseContext(options);
        return Task.CompletedTask;
    }
    
    private async Task WaitForPostgresReadyAsync()
    {
        const int maxRetries = 10; 
        const int delayBetweenRetries = 1;
    
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                await DbContext!.Database.CanConnectAsync();
            
                await DbContext.Database.ExecuteSqlRawAsync("SELECT 1");
                
                return;
            }
            catch
            {
                if (i == maxRetries - 1)
                {
                    throw new TimeoutException("PostgreSQL did not become ready");
                }
            
                await Task.Delay(TimeSpan.FromSeconds(delayBetweenRetries));
            }
        }
    }
    
    private async Task InitializeDatabaseAsync()
    {
        await DbContext!.Database.EnsureCreatedAsync();
    }
    
    private static string TakeDockerComposeString()
    {
        string projectRoot = AppContext.BaseDirectory;
        string[] pathItem = projectRoot.Split('/');
        string dockerComposePath = "";

        foreach (var item in pathItem)
        {
            dockerComposePath += item + "/";
            
            if (item == "auth")
                break;
        }
        
        return dockerComposePath + "docker-compose.test.yml";
    }
}