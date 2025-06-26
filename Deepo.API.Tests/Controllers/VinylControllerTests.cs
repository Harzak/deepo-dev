using Deepo.API.Controller;
using Deepo.Mediator.Query;
using FluentAssertions;
using Framework.Common.Utils.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.API.Tests.Controllers
{
    [TestClass]
    public class VinylControllerTests
    {
        private Mock<ILogger<VinylController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;

        private VinylController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _loggerMock = new Mock<ILogger<VinylController>>();
            _mediatorMock = new Mock<IMediator>();
            _controller = new VinylController(_mediatorMock.Object, _loggerMock.Object);
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
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetVinylReleaseQuery>(), CancellationToken.None)).ReturnsAsync(result);

            //Act
            IActionResult controllerResult = await _controller.Get(Guid.NewGuid(), CancellationToken.None);

            //Assert
            controllerResult.Should().NotBeNull();
            controllerResult.Should().BeAssignableTo(typeof(OkObjectResult));

            OkObjectResult badRequestResult = (OkObjectResult)controllerResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeAssignableTo(typeof(OperationResult<Dto.ReleaseVinylExDto>));

            OperationResult<Dto.ReleaseVinylExDto> customResult = (OperationResult<Dto.ReleaseVinylExDto>)badRequestResult.Value;
            customResult.Should().BeSameAs(result);
        }

        [TestMethod]
        public async Task GetByID_FailedRequest()
        {
            //Arrange
            OperationResult<Dto.ReleaseVinylExDto> result = new();
            result.WithError("something wrong :(");
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetVinylReleaseQuery>(), CancellationToken.None)).ReturnsAsync(result);

            //Act
            IActionResult controllerResult = await _controller.Get(Guid.NewGuid(), CancellationToken.None);

            //Assert
            controllerResult.Should().NotBeNull();
            controllerResult.Should().BeAssignableTo(typeof(BadRequestObjectResult));

            BadRequestObjectResult badRequestResult = (BadRequestObjectResult)controllerResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeAssignableTo(typeof(OperationResult<Dto.ReleaseVinylExDto>));

            OperationResult<Dto.ReleaseVinylExDto> customResult = (OperationResult<Dto.ReleaseVinylExDto>)badRequestResult.Value;
            customResult.Should().BeSameAs(result);
        }

        [TestMethod]
        public async Task GetByID_NoContent()
        {
            //Arrange
            OperationResult<Dto.ReleaseVinylExDto> result = new();
            result.WithSuccess();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetVinylReleaseQuery>(), CancellationToken.None)).ReturnsAsync(result);

            //Act
            IActionResult controllerResult = await _controller.Get(Guid.NewGuid(), CancellationToken.None);

            //Assert
            controllerResult.Should().NotBeNull();
            controllerResult.Should().BeAssignableTo(typeof(NoContentResult));
        }

        [TestMethod]
        public async Task GetByID_Null()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetVinylReleaseQuery>(), CancellationToken.None)).ReturnsAsync((OperationResult<Dto.ReleaseVinylExDto>)null);

            //Act
            IActionResult controllerResult = await _controller.Get(Guid.NewGuid(), CancellationToken.None);

            //Assert
            controllerResult.Should().NotBeNull();
            controllerResult.Should().BeAssignableTo(typeof(BadRequestObjectResult));

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
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllVinylReleasesWithPaginationQuery>(), CancellationToken.None)).ReturnsAsync(result);

            //Act
            IActionResult controllerResult = await _controller.Get("fr", 50, 300, CancellationToken.None);

            //Assert
            controllerResult.Should().NotBeNull();
            controllerResult.Should().BeAssignableTo(typeof(OkObjectResult));

            OkObjectResult badRequestResult = (OkObjectResult)controllerResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeAssignableTo(typeof(OperationResultList<Dto.ReleaseVinylDto>));

            OperationResultList<Dto.ReleaseVinylDto> customResult = (OperationResultList<Dto.ReleaseVinylDto>)badRequestResult.Value;
            customResult.Should().BeSameAs(result);
        }

        [TestMethod]
        public async Task GetList_FailedRequest()
        {
            //Arrange
            OperationResultList<Dto.ReleaseVinylDto> result = new();
            result.WithError("something wrong :(");
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllVinylReleasesWithPaginationQuery>(), CancellationToken.None)).ReturnsAsync(result);

            //Act
            IActionResult controllerResult = await _controller.Get("fr", 50, 300, CancellationToken.None);

            //Assert
            controllerResult.Should().NotBeNull();
            controllerResult.Should().BeAssignableTo(typeof(BadRequestObjectResult));

            BadRequestObjectResult badRequestResult = (BadRequestObjectResult)controllerResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().NotBeNull();
            badRequestResult.Value.Should().BeAssignableTo(typeof(OperationResultList<Dto.ReleaseVinylDto>));

            OperationResultList<Dto.ReleaseVinylDto> customResult = (OperationResultList<Dto.ReleaseVinylDto>)badRequestResult.Value;
            customResult.Should().BeSameAs(result);
        }

        [TestMethod]
        public async Task GetList_NoContent()
        {
            //Arrange
            OperationResultList<Dto.ReleaseVinylDto> result = new();
            result.WithSuccess();
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllVinylReleasesWithPaginationQuery>(), CancellationToken.None)).ReturnsAsync(result);

            //Act
            IActionResult controllerResult = await _controller.Get("fr", 50, 300, CancellationToken.None);

            //Assert
            controllerResult.Should().NotBeNull();
            controllerResult.Should().BeAssignableTo(typeof(NoContentResult));
        }

        [TestMethod]
        public async Task GetList_Null()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllVinylReleasesWithPaginationQuery>(), CancellationToken.None)).ReturnsAsync((OperationResultList<Dto.ReleaseVinylDto>)null);

            //Act
            IActionResult controllerResult = await _controller.Get("fr", 50, 300, CancellationToken.None);

            //Assert
            controllerResult.Should().NotBeNull();
            controllerResult.Should().BeAssignableTo(typeof(BadRequestObjectResult));

            BadRequestObjectResult badRequestResult = (BadRequestObjectResult)controllerResult;
            badRequestResult.Should().NotBeNull();
        }
    }
}
