using Domain.Common;
using System.Diagnostics.CodeAnalysis;

namespace InMetrics.Domain.Tests.Common;

[ExcludeFromCodeCoverage]
public class BaseAuditableEntityTests
{
    private class TestAuditableEntity : BaseAuditableEntity
    {
    }

    [Fact]
    public void BaseAuditableEntity_DefaultConstructor_SetsCreated()
    {
        // Act
        var testEntity = new TestAuditableEntity();

        // Assert
        Assert.NotEqual(DateTime.MinValue, testEntity.Created);
    }
}