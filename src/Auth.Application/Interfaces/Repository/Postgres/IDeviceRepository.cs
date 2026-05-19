using Auth.Domain.Common;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Repository.Postgres;

public interface IDeviceRepository
{
    Task<Result<Device?>> GetById(Guid id);
    Task<Result<bool?>> AddAsync(Device device);
}