using FakeRepo.Test.GuidTests.AutomationLayer;
using FluentAssertions;
using NUnit.Framework;

namespace FakeRepo.Test.GuidTests;

public class SmurfTest
{
    private UserRepositoryFake _users;

    [SetUp]
    public void Setup()
    {
        _users = new UserRepositoryFake(new MicrosoftSerializer());
    }

    [Test]
    public void Given()
    {
        var user = _users.Add();

        _users.GetById(user.Id).Should().BeEquivalentTo(user);
    }

    [Test]
    public void Given2()
    {
        var user = _users.Add(e => e.Name = "Original Name");

        user.Name = "Other Name";

        _users.GetById(user.Id).Name.Should().Be("Original Name");
    }
}
