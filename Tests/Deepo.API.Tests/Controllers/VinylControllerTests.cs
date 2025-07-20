using Deepo.API.Controller;
using Deepo.Framework.Results;
using Deepo.Mediator.Query;
using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.API.Tests.Controllers;

[TestClass]
public class VinylControllerTests
{
    private ILogger<VinylController> _loggerMock;
    private IMediator _mediatorMock;

    private VinylController _controller;

    [TestInitialize]
    public void Initialize()
    {
        _loggerMock = A.Fake<ILogger<VinylController>>();
        _mediatorMock = A.Fake<IMediator>();
        _controller = new VinylController(_mediatorMock, _loggerMock);
    }

    [TestMethod]
    public async Task GetByID_Ok()
    {
        //Arrange
        var resultContent = new Dto.ReleaseVinylExDto()
        {
            Id = Guid.NewGuid(),
            Name ="name",
            ReleaseDate = DateTime.Now
        };
        resultContent.AuthorsNames.Add("test");
        OperationResult<Dto.ReleaseVinylExDto> result = new()
        {
            IsSuccess = true,
            Content = resultContent,
        };
        A.CallTo(() => _mediatorMock.Send(A<GetVinylReleaseQuery>._, CancellationToken.None)).Returns(Task.FromResult(result));

        //Act
        IActionResult controllerResult = await _controller.Get(Guid.NewGuid(), CancellationToken.None);

        //Assert
        controllerResult.Should().NotBeNull();
        controllerResult.Should().BeAssignableTo<OkObjectResult>();

        OkObjectResult badRequestResult = (OkObjectResult)controllerResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.Value.Should().NotBeNull();
        badRequestResult.Value.Should().BeAssignableTo<OperationResult<Dto.ReleaseVinylExDto>>();

        OperationResult<Dto.ReleaseVinylExDto> customResult = (OperationResult<Dto.ReleaseVinylExDto>)badRequestResult.Value;
        customResult.Should().BeSameAs(result);
    }

    [TestMethod]
    public async Task GetByID_FailedRequest()
    {
        //Arrange
        OperationResult<Dto.ReleaseVinylExDto> result = new();
        result.WithError("something wrong :(");
        A.CallTo(() => _mediatorMock.Send(A<GetVinylReleaseQuery>._, CancellationToken.None)).Returns(Task.FromResult(result));

        //Act
        IActionResult controllerResult = await _controller.Get(Guid.NewGuid(), CancellationToken.None);

        //Assert
        controllerResult.Should().NotBeNull();
        controllerResult.Should().BeAssignableTo<BadRequestObjectResult>();

        BadRequestObjectResult badRequestResult = (BadRequestObjectResult)controllerResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.Value.Should().NotBeNull();
        badRequestResult.Value.Should().BeAssignableTo<OperationResult<Dto.ReleaseVinylExDto>>();

        OperationResult<Dto.ReleaseVinylExDto> customResult = (OperationResult<Dto.ReleaseVinylExDto>)badRequestResult.Value;
        customResult.Should().BeSameAs(result);
    }

    [TestMethod]
    public async Task GetByID_NoContent()
    {
        //Arrange
        OperationResult<Dto.ReleaseVinylExDto> result = new();
        result.WithSuccess();
        A.CallTo(() => _mediatorMock.Send(A<GetVinylReleaseQuery>._, CancellationToken.None)).Returns(Task.FromResult(result));

        //Act
        IActionResult controllerResult = await _controller.Get(Guid.NewGuid(), CancellationToken.None);

        //Assert
        controllerResult.Should().NotBeNull();
        controllerResult.Should().BeAssignableTo<NoContentResult>();
    }

    [TestMethod]
    public async Task GetByID_Null()
    {
        //Arrange
        A.CallTo(() => _mediatorMock.Send(A<GetVinylReleaseQuery>._, CancellationToken.None)).Returns(Task.FromResult((OperationResult<Dto.ReleaseVinylExDto>)null));

        //Act
        IActionResult controllerResult = await _controller.Get(Guid.NewGuid(), CancellationToken.None);

        //Assert
        controllerResult.Should().NotBeNull();
        controllerResult.Should().BeAssignableTo<BadRequestObjectResult>();

        BadRequestObjectResult badRequestResult = (BadRequestObjectResult)controllerResult;
        badRequestResult.Should().NotBeNull();
    }

    [TestMethod]
    public async Task GetList_Ok()
    {
        //Arrange
        var resultContent = new List<Dto.ReleaseVinylDto>() {
                new Dto.ReleaseVinylDto() {
                    Id = Guid.NewGuid(),
                    Name ="name",
                    ReleaseDate = DateTime.Now
                },
                new Dto.ReleaseVinylDto() {
                    Id = Guid.NewGuid(),
                    Name ="name",
                    ReleaseDate = DateTime.Now
                }
            };
        OperationResultList<Dto.ReleaseVinylDto> result = new()
        {
            IsSuccess = true,
            Content = resultContent
        };
        A.CallTo(() => _mediatorMock.Send(A<GetAllVinylReleasesWithPaginationQuery>._, CancellationToken.None)).Returns(Task.FromResult(result));

        //Act
        IActionResult controllerResult = await _controller.Get("fr", 50, 300, CancellationToken.None);

        //Assert
        controllerResult.Should().NotBeNull();
        controllerResult.Should().BeAssignableTo<OkObjectResult>();

        OkObjectResult badRequestResult = (OkObjectResult)controllerResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.Value.Should().NotBeNull();
        badRequestResult.Value.Should().BeAssignableTo<OperationResultList<Dto.ReleaseVinylDto>>();

        OperationResultList<Dto.ReleaseVinylDto> customResult = (OperationResultList<Dto.ReleaseVinylDto>)badRequestResult.Value;
        customResult.Should().BeSameAs(result);
    }

    [TestMethod]
    public async Task GetList_FailedRequest()
    {
        //Arrange
        OperationResultList<Dto.ReleaseVinylDto> result = new();
        result.WithError("something wrong :(");
        A.CallTo(() => _mediatorMock.Send(A<GetAllVinylReleasesWithPaginationQuery>._, CancellationToken.None)).Returns(Task.FromResult(result));

        //Act
        IActionResult controllerResult = await _controller.Get("fr", 50, 300, CancellationToken.None);

        //Assert
        controllerResult.Should().NotBeNull();
        controllerResult.Should().BeAssignableTo<BadRequestObjectResult>();

        BadRequestObjectResult badRequestResult = (BadRequestObjectResult)controllerResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.Value.Should().NotBeNull();
        badRequestResult.Value.Should().BeAssignableTo<OperationResultList<Dto.ReleaseVinylDto>>();

        OperationResultList<Dto.ReleaseVinylDto> customResult = (OperationResultList<Dto.ReleaseVinylDto>)badRequestResult.Value;
        customResult.Should().BeSameAs(result);
    }

    [TestMethod]
    public async Task GetList_NoContent()
    {
        //Arrange
        OperationResultList<Dto.ReleaseVinylDto> result = new();
        result.WithSuccess();
        A.CallTo(() => _mediatorMock.Send(A<GetAllVinylReleasesWithPaginationQuery>._, CancellationToken.None)).Returns(Task.FromResult(result));

        //Act
        IActionResult controllerResult = await _controller.Get("fr", 50, 300, CancellationToken.None);

        //Assert
        controllerResult.Should().NotBeNull();
        controllerResult.Should().BeAssignableTo<NoContentResult>();
    }

    [TestMethod]
    public async Task GetList_Null()
    {
        //Arrange
        A.CallTo(() => _mediatorMock.Send(A<GetAllVinylReleasesWithPaginationQuery>._, CancellationToken.None)).Returns(Task.FromResult((OperationResultList<Dto.ReleaseVinylDto>)null));

        //Act
        IActionResult controllerResult = await _controller.Get("fr", 50, 300, CancellationToken.None);

        //Assert
        controllerResult.Should().NotBeNull();
        controllerResult.Should().BeAssignableTo<BadRequestObjectResult>();

        BadRequestObjectResult badRequestResult = (BadRequestObjectResult)controllerResult;
        badRequestResult.Should().NotBeNull();
    }
}
