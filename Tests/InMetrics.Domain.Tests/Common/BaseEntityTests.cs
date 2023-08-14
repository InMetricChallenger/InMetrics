using Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace InMetrics.Domain.Tests.Common;

[ExcludeFromCodeCoverage]
public class BaseEntityTests
{
    private class TestEntity : BaseEntity
    {
        public TestEntity() : base() { }
        public TestEntity(int id) : base(id) { }
    }

    [Fact]
    public void BaseEntity_Constructor_SetsId()
    {
        // Arrange
        int expectedId = 1;

        // Act
        var testEntity = new TestEntity(expectedId);

        // Assert
        Assert.Equal(expectedId, testEntity.Id);
    }
}
