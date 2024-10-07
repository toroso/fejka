using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FakeRepo.Test.GuidDtoTests.AutomationLayer;
using FakeRepo.Test.GuidDtoTests.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace FakeRepo.Test.GuidDtoTests;

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
}
