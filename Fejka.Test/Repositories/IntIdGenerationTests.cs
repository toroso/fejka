using System.Threading.Tasks;
using Fejka.Test.Repositories.AutomationLayer;
using FluentAssertions;
using NUnit.Framework;

namespace Fejka.Test.Repositories;

public class IntIdGenerationTests : UserIntDtoRepositoryTestsBase
{
    [Test]
    public void Given_UserAddedToDatabase_Then_UserIsAssignedSequentialId()
    {
        var user1 = Add();
        var user2 = Add();

        user1.Id.Should().Be(1);
        user2.Id.Should().Be(2);
    }

    [Test]
    public async Task Given_NewUser_When_Inserted_Then_UserIsAssignedSequentialId()
    {
        var user1 = DomainBuilder.CreateIntUser();
        var user2 = DomainBuilder.CreateIntUser();

        var id1 = await InsertAsync(user1);
        var id2 = await InsertAsync(user2);

        id1.Should().Be(1);
        id2.Should().Be(2);
    }

    [Test]
    public async Task Given_NewUser_When_Upserted_Then_UserIsAssignedSequentialId()
    {
        var user1 = DomainBuilder.CreateIntUser();
        var user2 = DomainBuilder.CreateIntUser();

        var id1 = await UpsertAsync(user1);
        var id2 = await UpsertAsync(user2);

        id1.Should().Be(1);
        id2.Should().Be(2);
    }

    [Test]
    public async Task Given_ExistingUser_When_Updated_Then_IdIsPreserved()
    {
        var user = Add();

        user.Name = "Updated Name";
        await UpdateAsync(user);

        var updatedUser = GetById(user.Id);
        updatedUser.Name.Should().Be("Updated Name");
    }

    [Test]
    public async Task Given_InsertedUsers_When_Updated_Then_SequentialIdGenerationContinues()
    {
        var user1 = Add();
        var user2 = Add();

        user1.Name = "Updated Name";
        await UpdateAsync(user1);

        var id3 = await InsertAsync(DomainBuilder.CreateIntUser());

        user1.Id.Should().Be(1);
        user2.Id.Should().Be(2);
        id3.Should().Be(3);
    }

    [Test]
    public async Task Given_MultipleOperations_When_UpsertingUsers_Then_IdsRemainUnique()
    {
        var user1 = Add();
        var user2 = Add();

        user1.Name = "Updated Name";
        var id1Again = await UpsertAsync(user1);

        user1.Id.Should().Be(id1Again);
        user1.Id.Should().NotBe(user2.Id);
    }

    [Test]
    public async Task Given_UserWithoutId_When_Inserted_Then_IdIsGeneratedAndNotEmpty()
    {
        var id = await InsertAsync(DomainBuilder.CreateIntUser(u => u.Id = 0));

        id.Should().Be(1);
        GetById(id).Id.Should().Be(id);
    }
}