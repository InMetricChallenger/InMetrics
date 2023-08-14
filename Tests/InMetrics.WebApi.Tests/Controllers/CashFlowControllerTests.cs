using Application.Features.CashFlows.Queries;
using FluentAssertions;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using WebApi.Controllers.V1;
using Application.Features.CashFlows.Commands;
using Application.Common.Models;
using Castle.Core.Resource;

namespace InMetrics.WebApi.Tests.Controllers;
public class CashFlowControllerTests
{
    private readonly AutoMocker _mocker;
    private readonly CashFlowController _controller;

    public CashFlowControllerTests()
    {
        _mocker = new AutoMocker(MockBehavior.Strict);
        var senderMock = _mocker.GetMock<ISender>();
        _controller = new CashFlowController(senderMock.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnCashFlow_WhenCashFlowExists()
    {
        int cashFlowId = 1;
        var cashFlowResponse = new CashFlowResponse(cashFlowId, 10, TransactionType.Debit, DateTime.UtcNow, DateTime.UtcNow);
        _mocker.GetMock<ISender>()
            .Setup(sender => sender.Send(It.Is<GetCashFlowByIdQuery>(q => q.CashFlowId == cashFlowId), CancellationToken.None))
            .ReturnsAsync(cashFlowResponse);

        var result = await _controller.GetById(cashFlowId, CancellationToken.None);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedCashFlow = okResult.Value.Should().BeOfType<CashFlowResponse>().Subject;
        returnedCashFlow.Should().BeEquivalentTo(cashFlowResponse);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedCashFlowId_WhenRequestIsValid()
    {
        var command = new CreateCashFlowCommand(10, TransactionType.Debit, DateTime.UtcNow);
        int createdCashFlowId = 1;

        _mocker.GetMock<ISender>()
            .Setup(sender => sender.Send(command, CancellationToken.None))
            .ReturnsAsync(ResultFactory.Success(createdCashFlowId));

        var result = await _controller.Create(command, CancellationToken.None);

        var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        var resultValue = createdAtActionResult.Value as Result<int>;
        resultValue.Should().NotBeNull();
        resultValue.Data.Should().Be(createdCashFlowId);
    }
}
