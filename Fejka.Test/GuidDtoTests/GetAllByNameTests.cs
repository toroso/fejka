using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Fejka.Test.GuidDtoTests;

[TestFixture]
public class GetAllByNameTests : UserRepositoryTestsBase
{
    [Test]
    public async Task Given_MultipleUsersWithSameName_When_GetAllByName_Then_ReturnsCorrectUsers()
    {
        var user1 = Add(e => e.Name = "John Doe");
        var user2 = Add(e => e.Name = "John Doe");
        var user3 = Add(e => e.Name = "Jane Doe");

        var result = await GetAllByNameAsync("John Doe");

        result.Should().BeEquivalentTo(new[] { user1, user2 });
    }

    [Test]
    public async Task Given_NoUsersWithMatchingName_When_GetAllByName_Then_ReturnsEmptyList()
    {
        Add(e => e.Name = "John Doe");

        var result = await GetAllByNameAsync("Jane Doe");

        result.Should().BeEmpty();
    }

    [Test]
    public async Task Given_RepositoryWithNoUsers_When_GetAllByName_Then_ReturnsEmptyList()
    {
        var result = await GetAllByNameAsync("John Doe");

        result.Should().BeEmpty();
    }

    [Test]
    public async Task Given_UserWithNullName_When_GetAllByName_Then_HandlesNullCorrectly()
    {
        var user = Add(e => e.Name = null);

        var result = await GetAllByNameAsync(null);

        result.Should().ContainSingle().Which.Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Given_MultipleUsersWithDifferentNames_When_GetAllByName_Then_ReturnsCorrectUser()
    {
        var user1 = Add(e => e.Name = "Alice");
        var user2 = Add(e => e.Name = "Bob");
        var user3 = Add(e => e.Name = "Charlie");

        var result = await GetAllByNameAsync("Bob");

        result.Should().ContainSingle().Which.Should().BeEquivalentTo(user2);
    }
}
