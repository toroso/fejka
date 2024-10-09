using System;
using System.Threading;
using System.Threading.Tasks;
using Fejka.Test.Repositories.AutomationLayer;
using Fejka.Test.Repositories.Domain;
using NUnit.Framework;

namespace Fejka.Test.Repositories;

public abstract class UserIntDtoRepositoryTestsBase
{
    private UserIntDtoRepositoryFake _users;

    [SetUp]
    public void Setup()
    {
        _users = new UserIntDtoRepositoryFake(new MicrosoftSerializer());
    }
    
    protected async Task<int> InsertAsync(UserIntDto user) => await _users.InsertAsync(user, CancellationToken.None);
    protected async Task UpdateAsync(UserIntDto user) => await _users.UpdateAsync(user, CancellationToken.None);
    protected async Task<int> UpsertAsync(UserIntDto user) => await _users.UpsertAsync(user, CancellationToken.None);

    protected UserIntDto Add(Action<UserIntDto> customizeAction = null) => _users.Add(customizeAction);
    protected UserIntDto GetById(int id) => _users.GetById(id);
}