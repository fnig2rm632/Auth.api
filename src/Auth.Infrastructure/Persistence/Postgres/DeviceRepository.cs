using Auth.Application.Interfaces.Repository.Postgres;
using Auth.Domain.Common;
using Auth.Domain.Entities;
using Auth.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Auth.Infrastructure.Persistence.Postgres;

public class DeviceRepository(DatabaseContext context) : IDeviceRepository
{
    public async Task<Result<Device?>> GetById(Guid id)
    {
        try
        {
            var device = await context.Devices
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);
                
            return device;
        }
        catch (NpgsqlException)
        {
            return Error.Database.ConnectionFailed;
        }
        catch (TimeoutException)
        {
            return Error.Database.TimeoutGateway;
        }
        catch (Exception)
        {
            return Error.Database.InternalServer;
        }
    }

    public async Task<Result<bool?>> AddAsync(Device device)
    {
        try
        {
            await context.Devices.AddAsync(device);
            await context.SaveChangesAsync();
            
            return true;
        }
        catch (NpgsqlException)
        {
            return Error.Database.ConnectionFailed;
        }
        catch (TimeoutException)
        {
            return Error.Database.TimeoutGateway;
        }
        catch (Exception)
        {
            return Error.Database.InternalServer;
        }
    }
}