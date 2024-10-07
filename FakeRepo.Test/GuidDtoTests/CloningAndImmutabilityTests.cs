using FakeRepo.Test.GuidDtoTests.AutomationLayer;
using FakeRepo.Test.GuidDtoTests.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace FakeRepo.Test.GuidDtoTests;

[TestFixture]
public class CloningAndImmutabilityTests : UserRepositoryTestsBase
{
    [Test]
    public void Given_UserInDatabase_When_ModifyingFetchedUser_Then_DatabaseRemainsUnchanged()
    {
        var user = Add(e => e.Name = "Original Name");

        var fetchedUser = GetById(user.Id);
        fetchedUser.Name = "Modified Name";

        GetById(user.Id).Name.Should().Be("Original Name");
    }

    [Test]
    public void Given_UserWithNestedObjects_When_ModifyingNestedObject_Then_DatabaseRemainsUnchanged()
    {
        var user = Add(e => e.HomeAddress.City = "Springfield");

        var fetchedUser = GetById(user.Id);
        fetchedUser.HomeAddress.City = "Shelbyville";

        GetById(user.Id).HomeAddress.City.Should().Be("Springfield");
    }

    [Test]
    public void Given_FetchedUser_When_ModifyingCollectionInMemory_Then_DatabaseRemainsUnchanged()
    {
        var user = Add(e => e.WorkAddress = new Address { Street = "456 Oak St", City = "Shelbyville" });

        var fetchedUser = GetById(user.Id);
        fetchedUser.WorkAddress.Street = "789 Maple St";

        GetById(user.Id).WorkAddress.Street.Should().Be("456 Oak St");
    }
}
