using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fejka.Repository;
using Fejka.Test.Repositories.Domain;

namespace Fejka.Test.Repositories.AutomationLayer;

public interface IUserIntDtoRepository
{
    Task<UserIntDto> GetByIdAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<UserIntDto>> GetAllByNameAsync(string name, CancellationToken ct);
    Task<int> InsertAsync(UserIntDto entity, CancellationToken ct);
    Task UpdateAsync(UserIntDto entity, CancellationToken ct);
    Task<int> UpsertAsync(UserIntDto entity, CancellationToken ct);
    Task DeleteByIdAsync(int id, CancellationToken ct);
}

public class UserIntDtoRepositoryFake : IntRepositoryFake<UserIntDto>, IUserIntDtoRepository
{
    public UserIntDtoRepositoryFake(ISerializer serializer)
        : base(serializer, cfg => cfg.GetIdFunc = entity => entity.Id)
    {
    }

    public Task<IReadOnlyList<UserIntDto>> GetAllByNameAsync(string name, CancellationToken ct)
        => GetAllByAsync(e => e.Name == name, ct);

    public UserIntDto Add(Action<UserIntDto> customizeAction = null)
        => base.Add(id => DomainBuilder.CreateIntUser(id, customizeAction));
}