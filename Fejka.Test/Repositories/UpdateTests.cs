using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fejka.Test.Repositories.AutomationLayer;
using FluentAssertions;
using NUnit.Framework;

namespace Fejka.Test.Repositories;

[TestFixture]
[SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
public class UpdateTests : UserGuidDtoRepositoryTestsBase
{
    [Test]
    public async Task Given_ExistingUser_When_Update_Then_UserIsUpdatedInDatabase()
    {
        var user = Add(e => e.Name = "John Doe");

        user.Name = "John Smith";
        await UpdateAsync(user);

        GetById(user.Id).Name.Should().Be("John Smith");
    }

    [Test]
    public async Task Given_NonExistentUser_When_Update_Then_NothingIsChanged()
    {
        var nonExistentUser = DomainBuilder.CreateGuidUser();

        await UpdateAsync(nonExistentUser);

        GetById(nonExistentUser.Id).Should().BeNull();
    }
    
    [Test]
    public async Task Given_UpdatedUser_When_ModifyingUpdatedUser_Then_DatabaseRemainsUnchanged()
    {
        var user = Add(e => e.Name = "Original Name");
        user.Name = "Updated Name";
        await UpdateAsync(user);

        user.Name = "Modified Name";

        GetById(user.Id).Name.Should().Be("Updated Name");
    }

    [Test]
    public async Task Given_NullUser_When_Update_Then_ThrowsArgumentNullException()
    {
        await InvokingUpdateAsync(null).Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Given_UserWithNullFields_When_Update_Then_NullFieldsAreUpdatedCorrectly()
    {
        var user = Add(e => e.Name = "John Doe");

        user.Name = null;
        await UpdateAsync(user);

        GetById(user.Id).Name.Should().BeNull();
    }
}
