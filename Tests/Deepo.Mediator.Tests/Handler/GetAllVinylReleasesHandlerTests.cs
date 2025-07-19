using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Dto;
using Deepo.Framework.Results;
using Deepo.Mediator.Handler;
using Deepo.Mediator.Query;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deepo.Mediator.Tests.Handler;

[TestClass]
public class GetAllVinylReleasesHandlerTests
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
        var handler = new GetAllVinylReleasesHandler(_dbMock);
        var request = new GetAllVinylReleasesWithPaginationQuery("US")
        {
            Offset = 0,
            Limit = 10
        };
        IReadOnlyCollection<V_LastVinylRelease> releases = CreateTestDataReleases();
        A.CallTo(() => _dbMock.GetAllAsync(request.Market, request.Offset, request.Limit, default)).Returns(Task.FromResult(releases));

        //Act
        OperationResultList<ReleaseVinylDto> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ErrorCode.Should().BeNullOrEmpty();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.HasContent.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.Count.Should().Be(releases.Count);
        result.Content.First().Name.Should().Be(releases.First().AlbumName);
        result.Content.First().Id.Should().Be(releases.First().ReleasGUID);
        result.Content.First().ReleaseDate.Should().Be(releases.First().Release_Date_UTC);
        result.Content.First().Market.Should().Be(releases.First().Market);
        result.Content.First().CoverUrl.Should().Be(releases.First().Cover_URL);
        result.Content.First().Genres.Count.Should().Be(releases.First().GenresIdentifier.Split(';').Length);
        result.Content.First().AuthorsNames.Count.Should().Be(releases.First().ArtistsNames.Split(';').Length);
    }

    [TestMethod]
    public async Task HandleRequest_ShouldReturn_Success_WhenEmptyDatabaseData()
    {
        //Arrange
        var handler = new GetAllVinylReleasesHandler(_dbMock);
        var request = new GetAllVinylReleasesWithPaginationQuery("US")
        {
            Offset = 0,
            Limit = 10
        };
        IReadOnlyCollection<V_LastVinylRelease> releases = [];
        A.CallTo(() => _dbMock.GetAllAsync(request.Market, request.Offset, request.Limit, default)).Returns(Task.FromResult(releases));

        //Act
        OperationResultList<ReleaseVinylDto> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ErrorCode.Should().BeNullOrEmpty();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.HasContent.Should().BeFalse();
        result.Content.Should().BeEmpty();
    }

    [TestMethod]
    public async Task HandleRequest_ShouldThrow_WhenNoRequest()
    {
        //Arrange
        var handler = new GetAllVinylReleasesHandler(_dbMock);
        GetAllVinylReleasesWithPaginationQuery request = null;
        string exMessage = null;
        //Act
        try
        {
            OperationResultList<ReleaseVinylDto> result = await handler.Handle(request, default).ConfigureAwait(false);
        }
        catch (ArgumentNullException ex)
        {
            exMessage = ex.Message;
        }

        //Assert
        exMessage.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task HandleRequest_ShouldReturnError_WhenWrongRequest()
    {
        //Arrange
        var handler = new GetAllVinylReleasesHandler(_dbMock);
        var request = new GetAllVinylReleasesWithPaginationQuery(market: string.Empty)
        {
            Offset = -1,
            Limit = -5
        };

        //Act
        OperationResultList<ReleaseVinylDto> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.HasContent.Should().BeFalse();
        result.Content.Should().BeEmpty();
    }

    private IReadOnlyCollection<V_LastVinylRelease> CreateTestDataReleases()
    {
        return
        [
            new V_LastVinylRelease
            {
                ReleasGUID = "123e4567-e89b-12d3-a456-426614174000",
                AlbumName = "Test Album",
                Release_Date_UTC = DateTime.UtcNow,
                Market = "US",
                Cover_URL = "http://example.com/cover.jpg",
                GenresIdentifier = "123e4567-e89b-12d3-a456-426614174001;123e4567-e89b-12d3-a456-426614174002",
                ArtistsNames = "Artist One;Artist Two"
            }
        ];
    }
}
