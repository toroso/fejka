using System;
using FakeRepo.Test.GuidTests.Domain;

namespace FakeRepo.Test.GuidTests.AutomationLayer;

public static class DomainBuilder
{
    public static User Create(Action<User> customizeAction)
    {
        return DtoBuilder.Create(e =>
        {
            e.Id = Guid.NewGuid();
            e.Name = "John Doe";
            e.HomeAddress = new Address { Street = "123 Elm St", ZipCode = "62701", City = "Springfield" };
            e.WorkAddress = new Address { Street = "456 Oak St", ZipCode = "62702", City = "Shelbyville" };
        }, customizeAction);
    }
}