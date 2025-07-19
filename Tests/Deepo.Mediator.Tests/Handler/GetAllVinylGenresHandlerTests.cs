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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Mediator.Tests.Handler;

[TestClass]
public class GetAllVinylGenresHandlerTests
{
    private IGenreAlbumRepository _dbMock;

    [TestInitialize]
    public void Initialize()
    {
        _dbMock = A.Fake<IGenreAlbumRepository>();
    }

    [TestMethod]
    public async Task HandleRequest_ShouldReturn_Success_WhenDatabaseData()
    {
        //Arrange
        var handler = new GetAllVinylGenresHandler(_dbMock);
        var request = new GetAllVinylGenresQuery("rock");
        ReadOnlyCollection<Genre_Album> genres = CreateTestDataGenres();
        A.CallTo(() => _dbMock.GetAllAsync(default)).Returns(Task.FromResult(genres));

        //Act
        OperationResultList<GenreDto> result = await handler.Handle(request, default).ConfigureAwait(false);

        //Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.ErrorCode.Should().BeNullOrEmpty();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.HasContent.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.Count.Should().Be(genres.Count);
        result.Content.First().Name.Should().Be(genres.First().Name);
        result.Content.First().Identifier.Should().Be(Guid.Parse(genres.First().Identifier));
    }

    [TestMethod]
    public async Task HandleRequest_ShouldReturn_Success_WhenEmptyDatabaseData()
    {
        //Arrange
        var handler = new GetAllVinylGenresHandler(_dbMock);
        var request = new GetAllVinylGenresQuery(null);
        ReadOnlyCollection<Genre_Album> genres = new ReadOnlyCollection<Genre_Album>([]);
        A.CallTo(() => _dbMock.GetAllAsync(default)).Returns(Task.FromResult(genres));

        //Act
        OperationResultList<GenreDto> result = await handler.Handle(request, default).ConfigureAwait(false);

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
        var handler = new GetAllVinylGenresHandler(_dbMock);
        GetAllVinylGenresQuery request = null;
        string exMessage = null;

        //Act
        try
        {
            OperationResultList<GenreDto> result = await handler.Handle(request, default).ConfigureAwait(false);
        }
        catch (ArgumentNullException ex)
        {
            exMessage = ex.Message;
        }

        //Assert
        exMessage.Should().NotBeNullOrEmpty();
    }

    private ReadOnlyCollection<Genre_Album> CreateTestDataGenres()
    {
        return new ReadOnlyCollection<Genre_Album>(
        [
            new Genre_Album
            {
                Identifier = "123e4567-e89b-12d3-a456-426614174001",
                Name = "Rock"
            },
            new Genre_Album
            {
                Identifier = "123e4567-e89b-12d3-a456-426614174002",
                Name = "Jazz"
            },
            new Genre_Album
            {
                Identifier = "123e4567-e89b-12d3-a456-426614174003",
                Name = "Electronic"
            }
        ]);
    }
}
