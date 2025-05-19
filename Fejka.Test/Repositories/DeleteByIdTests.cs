using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Fejka.Test.Repositories;

[TestFixture]
[SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
public class DeleteByIdTests : UserGuidDtoRepositoryTestsBase
{
    [Test]
    public async Task Given_ExistingUserInDatabase_When_DeleteById_Then_UserIsDeleted()
    {
        // Arrange
        var user = Add();
        
        // Act
        await DeleteByIdAsync(user.Id);
        
        // Assert
        GetById(user.Id).Should().BeNull();
    }
    
    [Test]
    public async Task Given_NonExistentUser_When_DeleteById_Then_NoExceptionIsThrown()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        
        // Act & Assert
        await InvokingDeleteByIdAsync(nonExistentId).Should().NotThrowAsync();
    }
    
    [Test]
    public async Task Given_EmptyRepository_When_DeleteById_Then_NoExceptionIsThrown()
    {
        // Arrange
        var randomId = Guid.NewGuid();
        
        // Act & Assert
        await InvokingDeleteByIdAsync(randomId).Should().NotThrowAsync();
    }
    
    [Test]
    public async Task Given_MultipleUsersInDatabase_When_DeleteById_Then_OnlySpecifiedUserIsDeleted()
    {
        // Arrange
        var user1 = Add(e => e.Name = "User 1");
        var user2 = Add(e => e.Name = "User 2");
        var user3 = Add(e => e.Name = "User 3");
        
        // Act
        await DeleteByIdAsync(user2.Id);
        
        // Assert
        GetById(user1.Id).Should().NotBeNull();
        GetById(user2.Id).Should().BeNull();
        GetById(user3.Id).Should().NotBeNull();
    }
}
