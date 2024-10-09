using System;
using Fejka.Test.Repositories.Domain;

namespace Fejka.Test.Repositories.AutomationLayer;

public static class DomainBuilder
{
    public static UserGuidDto CreateGuidUser(Action<UserGuidDto> customizeAction = null)
    {
        return DtoBuilder.Create(e =>
        {
            e.Id = Guid.NewGuid();
            e.Name = "John Doe";
            e.HomeAddress = new Address { Street = "123 Elm St", ZipCode = "62701", City = "Springfield" };
            e.WorkAddress = new Address { Street = "456 Oak St", ZipCode = "62702", City = "Shelbyville" };
        }, customizeAction);
    }

    public static UserIntDto CreateIntUser(int id, Action<UserIntDto> customizeAction = null)
    {
        return CreateIntUser(e =>
        {
            e.Id = id;
            customizeAction?.Invoke(e);
        });
    }

    public static UserIntDto CreateIntUser(Action<UserIntDto> customizeAction = null)
    {
        return DtoBuilder.Create(e =>
        {
            e.Name = "John Doe";
            e.HomeAddress = new Address { Street = "123 Elm St", ZipCode = "62701", City = "Springfield" };
            e.WorkAddress = new Address { Street = "456 Oak St", ZipCode = "62702", City = "Shelbyville" };
        }, customizeAction);
    }
}