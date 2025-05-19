using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fejka.Test.Repositories.AutomationLayer;
using Fejka.Test.Repositories.Domain;
using NUnit.Framework;

namespace Fejka.Test.Repositories;

public abstract class UserGuidDtoRepositoryTestsBase
{
    private UserGuidDtoRepositoryFake _users;

    [SetUp]
    public void Setup()
    {
        _users = new UserGuidDtoRepositoryFake(new MicrosoftSerializer());
    }

    protected Task<UserGuidDto> GetByIdAsync(Guid id) => _users.GetByIdAsync(id, CancellationToken.None);

    protected Task<IReadOnlyList<UserGuidDto>> GetAllByNameAsync(string name) => _users.GetAllByNameAsync(name, CancellationToken.None);

    protected Task InsertAsync(UserGuidDto entity) => _users.InsertAsync(entity, CancellationToken.None);
    protected Func<Task> InvokingInsertAsync(UserGuidDto entity) => async () => await InsertAsync(entity);

    protected Task UpdateAsync(UserGuidDto entity) => _users.UpdateAsync(entity, CancellationToken.None);
    protected Func<Task> InvokingUpdateAsync(UserGuidDto entity) => async () => await UpdateAsync(entity);

    protected Task UpsertAsync(UserGuidDto entity) => _users.UpsertAsync(entity, CancellationToken.None);
    protected Func<Task> InvokingUpsertAsync(UserGuidDto entity) => async () => await UpsertAsync(entity);

    protected Task DeleteByIdAsync(Guid id) => _users.DeleteByIdAsync(id, CancellationToken.None);
    protected Func<Task> InvokingDeleteByIdAsync(Guid id) => async () => await DeleteByIdAsync(id);

    protected UserGuidDto Add(Action<UserGuidDto> customizeAction = null) => _users.Add(customizeAction);

    protected UserGuidDto GetById(Guid id) => _users.GetById(id);
}
