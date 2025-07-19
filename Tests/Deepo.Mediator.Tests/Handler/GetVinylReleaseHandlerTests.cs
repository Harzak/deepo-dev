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
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Mediator.Tests.Handler;

[TestClass]
public class GetVinylReleaseHandlerTests
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
        var handler = new GetVinylReleaseHandler(_dbMock);
        var testId = Guid.NewGuid();
        var request = new GetVinylReleaseQuery(testId);
        Release_Album release = CreateTestDataRelease();
        A.CallTo(() => _dbMock.GetByIdAsync(testId, default)).Returns(Task.FromResult(release));

        //Act
        OperationResult<ReleaseVinylExDto> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ErrorCode.Should().BeNullOrEmpty();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Content.Should().NotBeNull();
        result.Content.Id.Should().Be(Guid.Parse(release.Release.GUID));
        result.Content.Name.Should().Be(release.Release.Name);
        result.Content.ReleaseDate.Should().Be(release.Release.Release_Date_UTC);
        result.Content.Country.Should().Be(release.Country);
        result.Content.Market.Should().Be(release.Market);
        result.Content.Label.Should().Be(release.Label);
        result.Content.Genres.Count.Should().Be(release.Genre_Albums.Count);
        result.Content.AuthorsNames.Count.Should().Be(release.Release.Author_Releases.Count);
        result.Content.Tracklist.Count.Should().Be(release.Tracklist_Albums.Count);
    }

    [TestMethod]
    public async Task HandleRequest_ShouldReturn_Error_WhenEmptyId()
    {
        //Arrange
        var handler = new GetVinylReleaseHandler(_dbMock);
        var request = new GetVinylReleaseQuery(Guid.Empty);

        //Act
        OperationResult<ReleaseVinylExDto> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Content.Should().BeNull();
    }

    [TestMethod]
    public async Task HandleRequest_ShouldReturn_Error_WhenNoReleaseFound()
    {
        //Arrange
        var handler = new GetVinylReleaseHandler(_dbMock);
        var testId = Guid.NewGuid();
        var request = new GetVinylReleaseQuery(testId);
        Release_Album release = null;
        A.CallTo(() => _dbMock.GetByIdAsync(testId, default)).Returns(Task.FromResult(release));

        //Act
        OperationResult<ReleaseVinylExDto> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Content.Should().BeNull();
    }

    [TestMethod]
    public async Task HandleRequest_ShouldThrow_WhenNoRequest()
    {
        //Arrange
        var handler = new GetVinylReleaseHandler(_dbMock);
        GetVinylReleaseQuery request = null;
        string exMessage = null;

        //Act
        try
        {
            OperationResult<ReleaseVinylExDto> result = await handler.Handle(request, default).ConfigureAwait(false);
        }
        catch (ArgumentNullException ex)
        {
            exMessage = ex.Message;
        }

        //Assert
        exMessage.Should().NotBeNullOrEmpty();
    }

    private Release_Album CreateTestDataRelease()
    {
        return new Release_Album
        {
            Release_Album_ID = 1,
            Release_ID = 1,
            Country = "US",
            Market = "North America",
            Label = "Test Label",
            Release = new Release
            {
                GUID = "123e4567-e89b-12d3-a456-426614174001",
                Name = "Test Album",
                Release_Date_UTC = DateTime.UtcNow,
                Asset_Releases = new List<Asset_Release>
                {
                    new Asset_Release
                    {
                        Asset = new Asset
                        {
                            Content_URL = "http://example.com/cover.jpg"
                        }
                    }
                },
                Author_Releases = new List<Author_Release>
                {
                    new Author_Release
                    {
                        Author = new DAL.EF.Models.Author
                        {
                            Name = "Test Artist"
                        }
                    }
                }
            },
            Genre_Albums = new List<Genre_Album>
            {
                new Genre_Album
                {
                    Identifier = "123e4567-e89b-12d3-a456-426614174002",
                    Name = "Rock"
                }
            },
            Tracklist_Albums = new List<Tracklist_Album>
            {
                new Tracklist_Album
                {
                    Track_Album = new Track_Album
                    {
                        Title = "Test Track",
                        Duration = "3:45",
                        Position = 1
                    }
                }
            }
        };
    }
}