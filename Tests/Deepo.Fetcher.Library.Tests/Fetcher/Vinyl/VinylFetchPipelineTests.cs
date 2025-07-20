using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.Result;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Fetcher.Vinyl;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Results;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Deepo.Fetcher.Library.Tests.TestUtils;

namespace Deepo.Fetcher.Library.Tests.Fetcher.Vinyl;

[TestClass]
public class VinylFetchPipelineTests
{
    private IVinylStrategiesFactory _strategiesFactoryMock;
    private IReleaseAlbumRepository _releaseAlbumRepositoryMock;
    private IReleaseHistoryRepository _historyRepositoryMock;
    private ILogger<VinylFetchPipeline> _loggerMock;

    [TestInitialize]
    public void Initialize()
    {
        _strategiesFactoryMock = A.Fake<IVinylStrategiesFactory>();
        _releaseAlbumRepositoryMock = A.Fake<IReleaseAlbumRepository>();
        _historyRepositoryMock = A.Fake<IReleaseHistoryRepository>();
        _loggerMock = A.Fake<ILogger<VinylFetchPipeline>>();
    }

    [TestMethod]
    public void Constructor_ShouldCreatePipeline_WhenValidDependencies()
    {
        // Act
        VinylFetchPipeline pipeline = new VinylFetchPipeline(
            _strategiesFactoryMock,
            _releaseAlbumRepositoryMock,
            _historyRepositoryMock,
            _loggerMock);

        // Assert
        pipeline.Should().NotBeNull();
        pipeline.SuccessfulFetchCount.Should().Be(0);
        pipeline.FailedFetchCount.Should().Be(0);
        pipeline.IgnoredFetchCount.Should().Be(0);
        pipeline.FetchCount.Should().Be(0);
    }

    [TestMethod]
    public async Task StartAsync_ShouldProcessBothMarkets_WhenCalled()
    {
        // Arrange
        List<V_Spotify_Vinyl_Fetch_History> historyData = CreateTestHistoryData();
        IAsyncEnumerable<DtoSpotifyAlbum> frenchAlbums = CreateTestSpotifyAlbumsAsync();
        IAsyncEnumerable<DtoSpotifyAlbum> usAlbums = CreateTestSpotifyAlbumsAsync();

        A.CallTo(() => _historyRepositoryMock.GetSpotifyReleaseFetchHistoryByDateAsync(A<DateTime>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>(historyData)));

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyFrenchMarketAsync(A<CancellationToken>._))
            .Returns(frenchAlbums);

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyNorthAmericanMarketAsync(A<CancellationToken>._))
            .Returns(usAlbums);

        SetupSuccessfulDiscogsTitleSearch();
        SetupSuccessfulAlbumInsertion();

        VinylFetchPipeline pipeline = new VinylFetchPipeline(
            _strategiesFactoryMock,
            _releaseAlbumRepositoryMock,
            _historyRepositoryMock,
            _loggerMock);

        // Act
        await pipeline.StartAsync(CancellationToken.None);

        // Assert
        A.CallTo(() => _historyRepositoryMock.GetSpotifyReleaseFetchHistoryByDateAsync(A<DateTime>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyFrenchMarketAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyNorthAmericanMarketAsync(A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();

        pipeline.SuccessfulFetchCount.Should().Be(4);
    }

    [TestMethod]
    public async Task StartAsync_ShouldIgnoreAlbumsInHistory_WhenAlreadyProcessed()
    {
        // Arrange
        List<DtoSpotifyAlbum> testAlbums = CreateTestSpotifyAlbums();
        List<V_Spotify_Vinyl_Fetch_History> historyData =
        [
            new V_Spotify_Vinyl_Fetch_History
            {
                Identifier = testAlbums.First().Id
            }
        ];

        A.CallTo(() => _historyRepositoryMock.GetSpotifyReleaseFetchHistoryByDateAsync(A<DateTime>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>(historyData)));

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyFrenchMarketAsync(A<CancellationToken>._))
            .Returns(testAlbums.ToAsyncEnumerable());

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyNorthAmericanMarketAsync(A<CancellationToken>._))
            .Returns(AsyncUtils.EmptyAsyncEnumerable<DtoSpotifyAlbum>());

        VinylFetchPipeline pipeline = new VinylFetchPipeline(
            _strategiesFactoryMock,
            _releaseAlbumRepositoryMock,
            _historyRepositoryMock,
            _loggerMock);

        // Act
        await pipeline.StartAsync(CancellationToken.None);

        // Assert
        pipeline.IgnoredFetchCount.Should().Be(1);
        pipeline.SuccessfulFetchCount.Should().Be(0);
        pipeline.FailedFetchCount.Should().Be(1); 
    }

    [TestMethod]
    public async Task StartAsync_ShouldUseArtistSearchFallback_WhenTitleSearchFails()
    {
        // Arrange
        List<DtoSpotifyAlbum> testAlbums = CreateTestSpotifyAlbums();

        A.CallTo(() => _historyRepositoryMock.GetSpotifyReleaseFetchHistoryByDateAsync(A<DateTime>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>(new List<V_Spotify_Vinyl_Fetch_History>())));

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyFrenchMarketAsync(A<CancellationToken>._))
            .Returns(testAlbums.ToAsyncEnumerable());

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyNorthAmericanMarketAsync(A<CancellationToken>._))
            .Returns(AsyncUtils.EmptyAsyncEnumerable<DtoSpotifyAlbum>());
        A.CallTo(() => _strategiesFactoryMock.SearchDiscogsByTitleAsync(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new OperationResultList<DtoDiscogsRelease>().WithFailure()));

        SetupSuccessfulDiscogsArtistSearch();
        SetupSuccessfulAlbumInsertion();

        VinylFetchPipeline pipeline = new VinylFetchPipeline(
            _strategiesFactoryMock,
            _releaseAlbumRepositoryMock,
            _historyRepositoryMock,
            _loggerMock);

        // Act
        await pipeline.StartAsync(CancellationToken.None);

        // Assert
        pipeline.SuccessfulFetchCount.Should().Be(2);
        pipeline.FailedFetchCount.Should().Be(0);

        A.CallTo(() => _strategiesFactoryMock.SearchDiscogsByTitleAsync(A<string>._, A<CancellationToken>._))
            .MustHaveHappened(testAlbums.Count, Times.Exactly);
        A.CallTo(() => _strategiesFactoryMock.SearchDiscogsByArtistAsync(A<string>._, A<CancellationToken>._))
            .MustHaveHappened();
    }

    [TestMethod]
    public async Task StartAsync_ShouldIncrementFailedCount_WhenAllStrategiesFail()
    {
        // Arrange
        List<DtoSpotifyAlbum> testAlbums = CreateTestSpotifyAlbums();

        A.CallTo(() => _historyRepositoryMock.GetSpotifyReleaseFetchHistoryByDateAsync(A<DateTime>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>(new List<V_Spotify_Vinyl_Fetch_History>())));

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyFrenchMarketAsync(A<CancellationToken>._))
            .Returns(testAlbums.ToAsyncEnumerable());

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyNorthAmericanMarketAsync(A<CancellationToken>._))
            .Returns(AsyncUtils.EmptyAsyncEnumerable<DtoSpotifyAlbum>());

        A.CallTo(() => _strategiesFactoryMock.SearchDiscogsByTitleAsync(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new OperationResultList<DtoDiscogsRelease>().WithFailure()));

        A.CallTo(() => _strategiesFactoryMock.SearchDiscogsByArtistAsync(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new OperationResultList<DtoDiscogsRelease>().WithFailure()));

        VinylFetchPipeline pipeline = new VinylFetchPipeline(
            _strategiesFactoryMock,
            _releaseAlbumRepositoryMock,
            _historyRepositoryMock,
            _loggerMock);

        // Act
        await pipeline.StartAsync(CancellationToken.None);

        // Assert
        pipeline.FailedFetchCount.Should().Be(2);
        pipeline.SuccessfulFetchCount.Should().Be(0);
        pipeline.IgnoredFetchCount.Should().Be(0);
    }

    [TestMethod]
    public async Task StartAsync_ShouldHandleInsertionFailure_AndContinueProcessing()
    {
        // Arrange
        List<DtoSpotifyAlbum> testAlbums = CreateTestSpotifyAlbums();

        A.CallTo(() => _historyRepositoryMock.GetSpotifyReleaseFetchHistoryByDateAsync(A<DateTime>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>(new List<V_Spotify_Vinyl_Fetch_History>())));

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyFrenchMarketAsync(A<CancellationToken>._))
            .Returns(testAlbums.ToAsyncEnumerable());

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyNorthAmericanMarketAsync(A<CancellationToken>._))
            .Returns(AsyncUtils.EmptyAsyncEnumerable<DtoSpotifyAlbum>());

        SetupSuccessfulDiscogsTitleSearch();

        A.CallTo(() => _releaseAlbumRepositoryMock.InsertAsync(A<AlbumModel>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new DatabaseOperationResult(false) { ErrorMessage = "Database error" }));

        VinylFetchPipeline pipeline = new VinylFetchPipeline(
            _strategiesFactoryMock,
            _releaseAlbumRepositoryMock,
            _historyRepositoryMock,
            _loggerMock);

        // Act
        await pipeline.StartAsync(CancellationToken.None);

        // Assert
        pipeline.FailedFetchCount.Should().Be(2);
        pipeline.SuccessfulFetchCount.Should().Be(0);
    }

    [TestMethod]
    public async Task StartAsync_ShouldAddAlbumsToHistory_WhenProcessing()
    {
        // Arrange
        List<DtoSpotifyAlbum> testAlbums = CreateTestSpotifyAlbums();

        A.CallTo(() => _historyRepositoryMock.GetSpotifyReleaseFetchHistoryByDateAsync(A<DateTime>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>(new List<V_Spotify_Vinyl_Fetch_History>())));

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyFrenchMarketAsync(A<CancellationToken>._))
            .Returns(testAlbums.ToAsyncEnumerable());

        A.CallTo(() => _strategiesFactoryMock.DiscoverSpotifyNorthAmericanMarketAsync(A<CancellationToken>._))
            .Returns(AsyncUtils.EmptyAsyncEnumerable<DtoSpotifyAlbum>());

        SetupSuccessfulDiscogsTitleSearch();
        SetupSuccessfulAlbumInsertion();

        VinylFetchPipeline pipeline = new VinylFetchPipeline(
            _strategiesFactoryMock,
            _releaseAlbumRepositoryMock,
            _historyRepositoryMock,
            _loggerMock);

        // Act
        await pipeline.StartAsync(CancellationToken.None);

        // Assert
        A.CallTo(() => _historyRepositoryMock.AddSpotifyReleaseFetchHistoryAsync(A<string>._, A<DateTime>._, A<CancellationToken>._))
            .MustHaveHappened(testAlbums.Count, Times.Exactly);
    }

    private List<DtoSpotifyAlbum> CreateTestSpotifyAlbums()
    {
        return new List<DtoSpotifyAlbum>
        {
            new DtoSpotifyAlbum
            {
                Id = "album1",
                Name = "Test Album 1",
                Artists = new List<DtoSpotifyArtist>
                {
                    new DtoSpotifyArtist { Id = "artist1", Name = "Test Artist 1" }
                }
            },
            new DtoSpotifyAlbum
            {
                Id = "album2",
                Name = "Test Album 2",
                Artists = new List<DtoSpotifyArtist>
                {
                    new DtoSpotifyArtist { Id = "artist2", Name = "Test Artist 2" }
                }
            }
        };
    }

    private async IAsyncEnumerable<DtoSpotifyAlbum> CreateTestSpotifyAlbumsAsync()
    {
        List<DtoSpotifyAlbum> albums = CreateTestSpotifyAlbums();
        foreach (DtoSpotifyAlbum album in albums)
        {
            yield return album;
        }
        await Task.CompletedTask;
    }

    private List<V_Spotify_Vinyl_Fetch_History> CreateTestHistoryData()
    {
        return new List<V_Spotify_Vinyl_Fetch_History>
        {
            new V_Spotify_Vinyl_Fetch_History
            {
                Identifier = "existing_album_id",
                Date_UTC = DateTime.UtcNow.AddDays(-1)
            }
        };
    }

    private List<DtoDiscogsRelease> CreateTestDiscogsReleases()
    {
        return new List<DtoDiscogsRelease>
        {
            new DtoDiscogsRelease
            {
                Id = 1,
                Title = "Test Release 1",
                Artists = new List<DtoDiscogsArtist>
                {
                    new DtoDiscogsArtist { Name = "Test Artist 1" }
                }
            },
            new DtoDiscogsRelease
            {
                Id = 2,
                Title = "Test Release 2",
                Artists = new List<DtoDiscogsArtist>
                {
                    new DtoDiscogsArtist { Name = "Test Artist 2" }
                }
            }
        };
    }

    private void SetupSuccessfulDiscogsTitleSearch()
    {
        List<DtoDiscogsRelease> releases = CreateTestDiscogsReleases();
        A.CallTo(() => _strategiesFactoryMock.SearchDiscogsByTitleAsync(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new OperationResultList<DtoDiscogsRelease>().WithSuccess().WithValue(releases)));
    }

    private void SetupSuccessfulDiscogsArtistSearch()
    {
        List<DtoDiscogsRelease> releases = CreateTestDiscogsReleases();
        A.CallTo(() => _strategiesFactoryMock.SearchDiscogsByArtistAsync(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new OperationResultList<DtoDiscogsRelease>().WithSuccess().WithValue(releases)));
    }

    private void SetupSuccessfulAlbumInsertion()
    {
        A.CallTo(() => _releaseAlbumRepositoryMock.InsertAsync(A<AlbumModel>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new DatabaseOperationResult(true) { RowAffected = 1 }));
    }
}