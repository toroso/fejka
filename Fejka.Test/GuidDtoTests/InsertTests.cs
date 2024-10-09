using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Fejka.Test.GuidDtoTests.AutomationLayer;
using Fejka.Test.GuidDtoTests.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace Fejka.Test.GuidDtoTests;

[TestFixture]
[SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
public class InsertTests : UserRepositoryTestsBase
{
    [Test]
    public async Task Given_NewUser_When_Insert_Then_UserIsAddedToDatabase()
    {
        var user = DomainBuilder.Create();

        await InsertAsync(user);

        GetById(user.Id).Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Given_UserWithDuplicateId_When_Insert_Then_ThrowsException()
    {
        var user = Add();

        var duplicateUser = DomainBuilder.Create(u => u.Id = user.Id);

        await InvokingInsertAsync(duplicateUser)
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Entity of type {typeof(UserGuidDto).Name} with id {user.Id} already exists*");
    }

    [Test]
    public async Task Given_NullUser_When_Insert_Then_ThrowsArgumentNullException()
    {
        await InvokingInsertAsync(null).Should().ThrowAsync<ArgumentNullException>();
    }
    
    [Test]
    public async Task Given_InsertedUser_When_ModifyingInsertedUser_Then_DatabaseRemainsUnchanged()
    {
        var user = DomainBuilder.Create(e => e.Name = "Original Name");
        await InsertAsync(user);

        user.Name = "Modified Name";

        GetById(user.Id).Name.Should().Be("Original Name");
    }
}
