using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fejka.Test.GuidDtoTests.AutomationLayer;
using FluentAssertions;
using NUnit.Framework;

namespace Fejka.Test.GuidDtoTests;

[TestFixture]
[SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
public class UpsertTests : UserRepositoryTestsBase
{
    [Test]
    public async Task Given_NewUser_When_Upsert_Then_UserIsInserted()
    {
        var user = DomainBuilder.Create();

        await UpsertAsync(user);

        GetById(user.Id).Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Given_ExistingUser_When_Upsert_Then_UserIsUpdated()
    {
        var user = Add(e => e.Name = "John Doe");

        user.Name = "John Smith";
        await UpsertAsync(user);

        GetById(user.Id).Name.Should().Be("John Smith");
    }

    [Test]
    public async Task Given_ExistingUserWithNewId_When_Upsert_Then_UserIsInsertedAndOldIdIsNotUpdated()
    {
        var user = Add(e => e.Name = "John Doe");

        var newId = Guid.NewGuid();
        user.Id = newId;

        await UpsertAsync(user);

        GetById(newId).Should().BeEquivalentTo(user);
        GetById(user.Id).Should().NotBeNull();
    }
    
    [Test]
    public async Task Given_UpsertedUser_When_ModifyingUpsertedUser_Then_DatabaseRemainsUnchanged()
    {
        var user = Add(e => e.Name = "Original Name");
        user.Name = "Upserted Name";
        await UpsertAsync(user);

        user.Name = "Modified Name";

        GetById(user.Id).Name.Should().Be("Upserted Name");
    }

    [Test]
    public async Task Given_NullUser_When_Upsert_Then_ThrowsArgumentNullException()
    {
        await InvokingUpsertAsync(null).Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task Given_UserWithNullFields_When_Upsert_Then_UserWithNullFieldsIsInsertedOrUpdatedCorrectly()
    {
        var user = Add(e => e.Name = "John Doe");

        user.Name = null;
        await UpsertAsync(user);

        GetById(user.Id).Name.Should().BeNull();
    }
}
