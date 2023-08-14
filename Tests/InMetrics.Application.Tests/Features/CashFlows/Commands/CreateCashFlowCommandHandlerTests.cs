using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.CashFlows.Commands;
using Castle.Core.Resource;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Moq;
using Polly;

namespace InMetrics.Application.Tests.Features.CashFlows.Commands;
public class CreateCashFlowCommandHandlerTests
{
    private readonly Mock<ICashFlowRepository> _cashFlowRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPollyPolicies> _pollyPoliciesMock;
    private readonly CreateCashFlowCommandHandler _handler;

    public CreateCashFlowCommandHandlerTests()
    {
        _cashFlowRepositoryMock = new Mock<ICashFlowRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mediatorMock = new Mock<IMediator>();
        _pollyPoliciesMock = new Mock<IPollyPolicies>();

        var asyncRetryPolicy = Policy.Handle<Exception>().RetryAsync(0);
        _pollyPoliciesMock.Setup(p => p.HandleDatabaseExceptions())
            .Returns(new AsyncRetryPolicyWrapper(asyncRetryPolicy));

        _handler = new CreateCashFlowCommandHandler(_cashFlowRepositoryMock.Object, _unitOfWorkMock.Object, _mediatorMock.Object);
    }


    [Fact]
    public async Task Handle_ShouldCreateCashFlow()
    {
        decimal amount = 10m;
        TransactionType transactionType = TransactionType.Debit;
        DateTime date = DateTime.Now;
        var createCashFlowCommand = new CreateCashFlowCommand(amount, transactionType, date);        
        var cancellationToken = CancellationToken.None;

        _cashFlowRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CashFlow>()))
           .Callback<CashFlow>(c => c.SetPrivatePropertyValue("Id", 1))
           .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync(cancellationToken))
            .Returns(Task.FromResult(1));

        Result<int> result = await _handler.Handle(createCashFlowCommand, cancellationToken);
        int cashFlowId = result.Data;

        Assert.True(result.Succeeded);
        _cashFlowRepositoryMock.Verify(x => x.AddAsync(It.IsAny<CashFlow>()), Times.Once);
        _mediatorMock.Verify(x => x.Publish(It.IsAny<INotification>(), cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);

        Assert.NotEqual(0, cashFlowId);
    }
}
