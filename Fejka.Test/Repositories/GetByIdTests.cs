using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Fejka.Test.Repositories;

[TestFixture]
[SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
public class GetByIdTests : UserGuidDtoRepositoryTestsBase
{
    [Test]
    public async Task Given_ExistingUserInDatabase_When_GetById_Then_UserIsReturned()
    {
        var user = Add();

        var fetchedUser = await GetByIdAsync(user.Id);

        fetchedUser.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Given_NonExistentUser_When_GetById_Then_NullIsReturned()
    {
        var fetchedUser = await GetByIdAsync(Guid.NewGuid());

        fetchedUser.Should().BeNull();
    }

    [Test]
    public async Task Given_EmptyRepository_When_GetById_Then_NullIsReturned()
    {
        var fetchedUser = await GetByIdAsync(Guid.NewGuid());

        fetchedUser.Should().BeNull();
    }

    [Test]
    public async Task Given_UserWithGeneratedId_When_GetById_Then_UserIsReturnedWithCorrectId()
    {
        var user = Add(e => e.Id = Guid.Empty);

        var fetchedUser = await GetByIdAsync(user.Id);

        fetchedUser.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Given_UserInDatabase_When_ModifyingFetchedUser_Then_DatabaseRemainsUnchanged()
    {
        var user = Add(e => e.Name = "Original Name");

        var fetchedUser = await GetByIdAsync(user.Id);
        fetchedUser.Name = "Modified Name";

        GetById(user.Id).Name.Should().Be("Original Name");
    }

    [Test]
    public void Given_UserInDatabase_When_ModifyingOriginalUser_Then_DatabaseRemainsUnchanged()
    {
        var user = Add(e => e.Name = "Original Name");
        user.Name = "Modified Name";

        GetById(user.Id).Name.Should().Be("Original Name");
    }

    [Test]
    public async Task Given_UserWithNestedObjects_When_ModifyingNestedObject_Then_DatabaseRemainsUnchanged()
    {
        var user = Add(e => e.HomeAddress.City = "Springfield");

        var fetchedUser = await GetByIdAsync(user.Id);
        fetchedUser.HomeAddress.City = "Shelbyville";

        GetById(user.Id).HomeAddress.City.Should().Be("Springfield");
    }
}
