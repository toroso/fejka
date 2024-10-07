using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace FakeRepo.Test.GuidDtoTests;

[TestFixture]
[SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
public class GetByIdTests : UserRepositoryTestsBase
{
    [Test]
    public void Given_ExistingUserInDatabase_When_GetById_Then_UserIsReturned()
    {
        var user = Add();

        GetById(user.Id).Should().BeEquivalentTo(user);
    }

    [Test]
    public void Given_NonExistentUser_When_GetById_Then_NullIsReturned()
    {
        GetById(Guid.NewGuid()).Should().BeNull();
    }

    [Test]
    public void Given_EmptyRepository_When_GetById_Then_NullIsReturned()
    {
        GetById(Guid.NewGuid()).Should().BeNull();
    }

    [Test]
    public void Given_UserWithGeneratedId_When_GetById_Then_UserIsReturnedWithCorrectId()
    {
        var user = Add(e => e.Id = Guid.Empty);

        GetById(user.Id).Should().BeEquivalentTo(user);
    }
}
