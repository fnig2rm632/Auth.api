using Auth.Domain.Common;
using Auth.Domain.Entities;

namespace Auth.Application.Interfaces.Repository.Postgres;

public interface ISessionRepository
{
    Task<Result<Session?>> GetByIdAsync(Guid id);
    Task<Result<List<Session?>>> GetListByIdUserAsync(Guid idUser);
    Task<Result<List<Session?>>> GetListByIdDeviceAsync(Guid idDevice);
    Task<Result<bool?>> AddAsync(Session session);
    Task<Result<bool?>> UpdateAsync(Session session);
}