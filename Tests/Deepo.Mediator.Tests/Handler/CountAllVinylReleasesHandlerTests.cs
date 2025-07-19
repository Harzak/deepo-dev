using Deepo.DAL.Repository.Interfaces;
using Deepo.Framework.Results;
using Deepo.Mediator.Handler;
using Deepo.Mediator.Query;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Mediator.Tests.Handler;

[TestClass]
public class CountAllVinylReleasesHandlerTests
{
    private IReleaseAlbumRepository _dbMock;

    [TestInitialize]
    public void Initialize()
    {
        _dbMock = A.Fake<IReleaseAlbumRepository>();
    }

    [TestMethod]
    public async Task HandleRequest_ShouldReturn_Success_WhenDatabaseData()
    {
        //Arrange
        var handler = new CountAllVinylReleasesHandler(_dbMock);
        var request = new CountAllVinylReleasesQuery("US");
        int expectedCount = 42;
        A.CallTo(() => _dbMock.CountAsync(request.Market, default)).Returns(Task.FromResult(expectedCount));

        //Act
        OperationResult<int> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ErrorCode.Should().BeNullOrEmpty();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.HasContent.Should().BeTrue();
        result.Content.Should().Be(expectedCount);
    }

    [TestMethod]
    public async Task HandleRequest_ShouldReturn_Error_WhenEmptyMarket()
    {
        //Arrange
        var handler = new CountAllVinylReleasesHandler(_dbMock);
        var request = new CountAllVinylReleasesQuery(string.Empty);

        //Act
        OperationResult<int> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.HasContent.Should().BeFalse();
        result.Content.Should().Be(0);
    }

    [TestMethod]
    public async Task HandleRequest_ShouldThrow_WhenNoRequest()
    {
        //Arrange
        var handler = new CountAllVinylReleasesHandler(_dbMock);
        CountAllVinylReleasesQuery request = null;
        string exMessage = null;

        //Act
        try
        {
            OperationResult<int> result = await handler.Handle(request, default).ConfigureAwait(false);
        }
        catch (ArgumentNullException ex)
        {
            exMessage = ex.Message;
        }

        //Assert
        exMessage.Should().NotBeNullOrEmpty();
    }
}