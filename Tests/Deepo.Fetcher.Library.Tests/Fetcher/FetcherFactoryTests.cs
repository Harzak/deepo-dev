using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Fetcher;
using Deepo.Fetcher.Library.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Deepo.Fetcher.Library.Tests.Fetcher
{
    //[ExcludeFromCodeCoverage]
    //[TestClass]
    //public class FetcherFactoryTests
    //{
    //    private Mock<IFetchFactory> _fetchFactoryMock;
    //    private Mock<ISpotifyRepository> _spotifyServiceMock;
    //    private Mock<IOptions<FetcherOptions>> _fetcherOptionsMock;
    //    private Mock<ILogger<FetcherFactory>> _loggerMock;
    //    private static FetcherOptions _fetcherOptions;

    //    private readonly FetcherFactory _fetcherFactory;


    //    [TestInitialize]
    //    public void TestInitialize()
    //    {
    //        _fetcherOptions = GetTestFetcherOptions();
    //        _fetchFactoryMock = new Mock<IFetchFactory>();
    //        _spotifyServiceMock = new Mock<ISpotifyRepository>();
    //        _fetcherOptionsMock = new Mock<IOptions<FetcherOptions>>();
    //        _loggerMock = new Mock<ILogger<FetcherFactory>>();
    //    }

    //    //[TestMethod]
    //    //[DataRow("FETCHER_MOVIE", _fetcherOptions.FetcherMovieId, _fetcherOptions.FetcherMovieName)]
    //    //public void CreateFetcher_Should_CreateValid_FetcherMovie(string code, string id, string name)
    //    //{
    //    //    //Arrange
    //    //    _fetcherOptionsMock.Setup(x => x.Value).Returns(_fetcherOptions);
    //    //    _fetcherFactory = new FetcherFactory(_fetchFactoryMock.Object, _spotifyServiceMock.Object, _fetcherOptionsMock.Object, _loggerMock.Object);

    //    //    //Act
    //    //    IWorker worker = _fetcherFactory.CreateFetcher("FETCHER_MOVIE");

    //    //    //Assert
    //    //    worker.Should().NotBeNull();
    //    //    worker.ID.Should().Be(id);
    //    //    worker.Name.Should().Be(name);
    //    //}

    //    private FetcherOptions GetTestFetcherOptions()
    //    {
    //        return new FetcherOptions()
    //        {
    //            FetcherMovieId = Guid.NewGuid().ToString(),
    //            FetcherMovieName = "FetcherMovieName",
    //            FetcherVinyleId = Guid.NewGuid().ToString(),
    //            FetcherVinyleName = "FetcherVinylName"
    //        };
    //    }
    //}
}
