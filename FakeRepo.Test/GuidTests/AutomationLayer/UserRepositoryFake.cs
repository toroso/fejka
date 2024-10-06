using System;
using FakeRepo.Core;
using FakeRepo.Test.GuidTests.Domain;

namespace FakeRepo.Test.GuidTests.AutomationLayer;

public class UserRepositoryFake : GuidRepositoryFake<User>
{
    public UserRepositoryFake(ISerializer serializer)
        : base(serializer, cfg => cfg.GetIdFunc = entity => entity.Id)
    {
    }

    public User Add(Action<User> customizeAction = null)
        => base.Add(_ => DomainBuilder.Create(customizeAction));
}