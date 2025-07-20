using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Strategies.Vinyl;
using Deepo.Fetcher.Library.Utils;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Tests.Strategies.Vinyl;

[TestClass]
public class SpotifyDiscoverByMarketStrategyTests
{
    private ISpotifyRepository _spotifyRepositoryMock;
    private ILogger _loggerMock;
    private SpotifyDiscoverByMarketStrategy _strategy;

    [TestInitialize]
    public void Initialize()
    {
        _spotifyRepositoryMock = A.Fake<ISpotifyRepository>();
        _loggerMock = A.Fake<ILogger>();
        _strategy = new SpotifyDiscoverByMarketStrategy(_spotifyRepositoryMock, _loggerMock);
    }

    [TestMethod]
    public void Constructor_ShouldCreateStrategy_WhenValidDependencies()
    {
        // Act
        SpotifyDiscoverByMarketStrategy strategy = new SpotifyDiscoverByMarketStrategy(_spotifyRepositoryMock, _loggerMock);

        // Assert
        strategy.Should().NotBeNull();
    }

    [TestMethod]
    public async Task DiscoverFrenchMarketAsync_ShouldReturnValidAlbums_WhenSuccessfulResults()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> successfulResults = CreateSuccessfulHttpServiceResults();
        SetupSpotifyRepositoryForMarket("FR", successfulResults);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverFrenchMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(2);
        results.Should().OnlyContain(a => a.Name != null);
        results.Should().OnlyContain(a => a.Id != null);
        results.Should().OnlyContain(a => a.Artists != null && a.Artists.Any());

        A.CallTo(() => _spotifyRepositoryMock.GetNewReleasesViaSearch("FR", A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task DiscoverNorthAmericanMarketAsync_ShouldReturnValidAlbums_WhenSuccessfulResults()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> successfulResults = CreateSuccessfulHttpServiceResults();
        SetupSpotifyRepositoryForMarket("US", successfulResults);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverNorthAmericanMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(2);
        results.Should().OnlyContain(a => a.Name != null);
        results.Should().OnlyContain(a => a.Id != null);
        results.Should().OnlyContain(a => a.Artists != null && a.Artists.Any());

        A.CallTo(() => _spotifyRepositoryMock.GetNewReleasesViaSearch("US", A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task DiscoverFrenchMarketAsync_ShouldFilterOutInvalidAlbums_WhenMixedResults()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> mixedResults = CreateMixedHttpServiceResults();
        SetupSpotifyRepositoryForMarket("FR", mixedResults);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverFrenchMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(1);
        results.First().Name.Should().Be("Valid Album");
        results.First().Id.Should().Be("valid_id");
    }

    [TestMethod]
    public async Task DiscoverNorthAmericanMarketAsync_ShouldFilterOutInvalidAlbums_WhenMixedResults()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> mixedResults = CreateMixedHttpServiceResults();
        SetupSpotifyRepositoryForMarket("US", mixedResults);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverNorthAmericanMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(1);
        results.First().Name.Should().Be("Valid Album");
        results.First().Id.Should().Be("valid_id");
    }

    [TestMethod]
    public async Task DiscoverFrenchMarketAsync_ShouldReturnEmpty_WhenAllResultsAreFailed()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> failedResults = CreateFailedHttpServiceResults();
        SetupSpotifyRepositoryForMarket("FR", failedResults);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverFrenchMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().BeEmpty();
    }

    [TestMethod]
    public async Task DiscoverNorthAmericanMarketAsync_ShouldReturnEmpty_WhenAllResultsAreFailed()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> failedResults = CreateFailedHttpServiceResults();
        SetupSpotifyRepositoryForMarket("US", failedResults);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverNorthAmericanMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().BeEmpty();
    }

    [TestMethod]
    public async Task DiscoverFrenchMarketAsync_ShouldFilterOutAlbumsWithNullArtists_WhenInvalidArtistData()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> resultsWithNullArtists = CreateResultsWithInvalidArtists();
        SetupSpotifyRepositoryForMarket("FR", resultsWithNullArtists);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverFrenchMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(1); 
        results.First().Name.Should().Be("Album With Valid Artists");
    }

    [TestMethod]
    public async Task DiscoverNorthAmericanMarketAsync_ShouldFilterOutAlbumsWithNullArtists_WhenInvalidArtistData()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> resultsWithNullArtists = CreateResultsWithInvalidArtists();
        SetupSpotifyRepositoryForMarket("US", resultsWithNullArtists);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverNorthAmericanMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(1); 
        results.First().Name.Should().Be("Album With Valid Artists");
    }

    [TestMethod]
    public async Task DiscoverFrenchMarketAsync_ShouldFilterOutAlbumsWithEmptyNames_WhenInvalidNameData()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> resultsWithEmptyNames = CreateResultsWithInvalidNames();
        SetupSpotifyRepositoryForMarket("FR", resultsWithEmptyNames);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverFrenchMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(1);
        results.First().Name.Should().Be("Valid Album Name");
    }

    [TestMethod]
    public async Task DiscoverFrenchMarketAsync_ShouldFilterOutAlbumsWithNullIds_WhenInvalidIdData()
    {
        // Arrange
        List<HttpServiceResult<DtoSpotifyAlbum>> resultsWithNullIds = CreateResultsWithInvalidIds();
        SetupSpotifyRepositoryForMarket("FR", resultsWithNullIds);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverFrenchMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().HaveCount(1);
        results.First().Id.Should().Be("valid_id");
    }

    [TestMethod]
    public async Task DiscoverFrenchMarketAsync_ShouldReturnEmpty_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        SetupSpotifyRepositoryForMarket("FR", []);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverFrenchMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().BeEmpty();
    }

    [TestMethod]
    public async Task DiscoverNorthAmericanMarketAsync_ShouldReturnEmpty_WhenRepositoryReturnsEmpty()
    {
        // Arrange
        SetupSpotifyRepositoryForMarket("US", []);

        List<DtoSpotifyAlbum> results = [];

        // Act
        await foreach (DtoSpotifyAlbum album in _strategy.DiscoverNorthAmericanMarketAsync(CancellationToken.None))
        {
            results.Add(album);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().BeEmpty();
    }

    [TestMethod]
    public async Task DiscoverFrenchMarketAsync_ShouldRespectCancellation_WhenTokenIsCancelled()
    {
        // Arrange
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.Cancel();

        A.CallTo(() => _spotifyRepositoryMock.GetNewReleasesViaSearch("FR", A<CancellationToken>._))
            .Returns(CreateCancelledAsyncEnumerable<HttpServiceResult<DtoSpotifyAlbum>>());

        // Act & Assert
        await Assert.ThrowsExactlyAsync<OperationCanceledException>(async () =>
        {
            await foreach (DtoSpotifyAlbum album in _strategy.DiscoverFrenchMarketAsync(cts.Token))
            {
                Assert.Fail("Should have been cancelled");
            }
        });
    }

    [TestMethod]
    public async Task DiscoverNorthAmericanMarketAsync_ShouldRespectCancellation_WhenTokenIsCancelled()
    {
        // Arrange
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.Cancel();

        A.CallTo(() => _spotifyRepositoryMock.GetNewReleasesViaSearch("US", A<CancellationToken>._))
            .Returns(CreateCancelledAsyncEnumerable<HttpServiceResult<DtoSpotifyAlbum>>());

        // Act & Assert
        await Assert.ThrowsExactlyAsync<OperationCanceledException>(async () =>
        {
            await foreach (DtoSpotifyAlbum album in _strategy.DiscoverNorthAmericanMarketAsync(cts.Token))
            {
                Assert.Fail("Should have been cancelled");
            }
        });
    }

    private List<HttpServiceResult<DtoSpotifyAlbum>> CreateSuccessfulHttpServiceResults()
    {
        return new List<HttpServiceResult<DtoSpotifyAlbum>>
        {
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "album1",
                    Name = "Test Album 1",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = "Test Artist 1" }
                    }
                }),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "album2",
                    Name = "Test Album 2",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist2", Name = "Test Artist 2" }
                    }
                })
        };
    }

    private List<HttpServiceResult<DtoSpotifyAlbum>> CreateMixedHttpServiceResults()
    {
        return new List<HttpServiceResult<DtoSpotifyAlbum>>
        {
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "valid_id",
                    Name = "Valid Album",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = "Test Artist" }
                    }
                }),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithFailure()
                .WithError("Failed to fetch album"),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = null, // Invalid - null ID
                    Name = "Invalid Album",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist2", Name = "Test Artist 2" }
                    }
                })
        };
    }

    private List<HttpServiceResult<DtoSpotifyAlbum>> CreateFailedHttpServiceResults()
    {
        return new List<HttpServiceResult<DtoSpotifyAlbum>>
        {
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithFailure()
                .WithError("API Error 1"),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithFailure()
                .WithError("API Error 2")
        };
    }

    private List<HttpServiceResult<DtoSpotifyAlbum>> CreateResultsWithInvalidArtists()
    {
        return new List<HttpServiceResult<DtoSpotifyAlbum>>
        {
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "album_with_null_artists",
                    Name = "Album With Null Artists",
                    Artists = null // Invalid - null artists
                }),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "album_with_empty_artists",
                    Name = "Album With Empty Artists",
                    Artists = [] // Invalid - empty artists
                }),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "album_with_null_artist_names",
                    Name = "Album With Null Artist Names",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = null } // Invalid - null artist name
                    }
                }),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "valid_album",
                    Name = "Album With Valid Artists",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = "Valid Artist" }
                    }
                })
        };
    }

    private List<HttpServiceResult<DtoSpotifyAlbum>> CreateResultsWithInvalidNames()
    {
        return new List<HttpServiceResult<DtoSpotifyAlbum>>
        {
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "album_with_null_name",
                    Name = null, // Invalid - null name
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = "Test Artist" }
                    }
                }),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "album_with_empty_name",
                    Name = "", // Invalid - empty name
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = "Test Artist" }
                    }
                }),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "valid_album",
                    Name = "Valid Album Name",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = "Test Artist" }
                    }
                })
        };
    }

    private List<HttpServiceResult<DtoSpotifyAlbum>> CreateResultsWithInvalidIds()
    {
        return new List<HttpServiceResult<DtoSpotifyAlbum>>
        {
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = null, // Invalid - null ID
                    Name = "Album With Null ID",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = "Test Artist" }
                    }
                }),
            new HttpServiceResult<DtoSpotifyAlbum>()
                .WithSuccess()
                .WithValue(new DtoSpotifyAlbum
                {
                    Id = "valid_id",
                    Name = "Album With Valid ID",
                    Artists = new List<DtoSpotifyArtist>
                    {
                        new DtoSpotifyArtist { Id = "artist1", Name = "Test Artist" }
                    }
                })
        };
    }

    private void SetupSpotifyRepositoryForMarket(string market, List<HttpServiceResult<DtoSpotifyAlbum>> results)
    {
        A.CallTo(() => _spotifyRepositoryMock.GetNewReleasesViaSearch(market, A<CancellationToken>._))
            .Returns(CreateAsyncEnumerable(results));
    }

    private static async IAsyncEnumerable<T> CreateAsyncEnumerable<T>(IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            yield return item;
        }
        await Task.CompletedTask;
    }

    private static async IAsyncEnumerable<T> CreateCancelledAsyncEnumerable<T>()
    {
        await Task.CompletedTask;
        throw new OperationCanceledException();
        yield break; // This will never be reached
    }
}