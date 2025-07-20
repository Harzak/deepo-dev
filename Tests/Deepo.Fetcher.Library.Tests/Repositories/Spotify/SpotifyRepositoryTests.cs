using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Repositories.Spotify;
using Deepo.Fetcher.Library.Tests.TestUtils;
using Deepo.Fetcher.Library.Utils;
using Deepo.Framework.Exceptions;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Web.Model;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Tests.Repositories.Spotify;

[TestClass]
public class SpotifyRepositoryTests
{
    private IHttpClientFactory _httpClientFactoryMock;
    private IAuthServiceFactory _authServiceFactoryMock;
    private IAuthenticationHttpService _authServiceMock;
    private IOptions<HttpServicesOption> _optionsMock;
    private ILogger<SpotifyRepository> _loggerMock;
    private HttpServicesOption _httpServicesOption;
    private HttpServiceOption _spotifyOption;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientFactoryMock = A.Fake<IHttpClientFactory>();
        _authServiceFactoryMock = A.Fake<IAuthServiceFactory>();
        _authServiceMock = A.Fake<IAuthenticationHttpService>();
        _optionsMock = A.Fake<IOptions<HttpServicesOption>>();
        _loggerMock = A.Fake<ILogger<SpotifyRepository>>();

        _spotifyOption = new HttpServiceOption
        {
            Name = "Spotify",
            BaseAddress = new Uri("https://api.spotify.com/"),
            UserAgent = "DeepoCatalog/1.0",
            TaskID = "spotify-task",
            ConsumerKey = "test_consumer_key",
            ConsumerSecret = "test_consumer_secret"
        };

        _httpServicesOption = new HttpServicesOption
        {
            Spotify = _spotifyOption
        };

        A.CallTo(() => _optionsMock.Value).Returns(_httpServicesOption);
        A.CallTo(() => _authServiceFactoryMock.CreateSpotifyAuthService()).Returns(_authServiceMock);
    }

    [TestMethod]
    public async Task GetNewReleasesViaSearch_ShouldReturn_Success_WhenValidResponse()
    {
        // Arrange
        IEnumerable<DtoSpotifyAlbum> expectedAlbums = CreateTestSpotifyAlbums();
        object albumsResponse = CreateSpotifyAlbumsResponse(expectedAlbums);
        string jsonResponse = JsonSerializer.Serialize(albumsResponse);

        TokenModel validToken = new TokenModel("valid_access_token", "Bearer", TimeSpan.FromHours(1));
        A.CallTo(() => _authServiceMock.RefreshTokenAsync(A<CancellationToken>._)).Returns(Task.FromResult<TokenModel>(validToken));

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        SpotifyRepository repository = new SpotifyRepository(_httpClientFactoryMock, _authServiceFactoryMock, _optionsMock, _loggerMock);

        // Act
        await foreach (HttpServiceResult<DtoSpotifyAlbum> result in repository.GetNewReleasesViaSearch("US", CancellationToken.None))
        {
            Assert(result); 
        }

        // Assert
        void Assert(HttpServiceResult<DtoSpotifyAlbum> result)
        {
            result.Should().NotBeNull();

            DtoSpotifyAlbum expected = expectedAlbums.FirstOrDefault(x => x.Id == result.Content?.Id);
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.HasContent.Should().BeTrue();
            result.Content.Name.Should().Be(expected.Name);
            result.Content.Id.Should().Be(expected.Id);
        }
    }

    [TestMethod]
    public async Task GetNewReleasesViaSearch_ShouldReturn_Empty_WhenNoResults()
    {
        // Arrange
        object albumsResponse = CreateSpotifyAlbumsResponse(new List<DtoSpotifyAlbum>());
        string jsonResponse = JsonSerializer.Serialize(albumsResponse);

        TokenModel validToken = new TokenModel("valid_access_token", "Bearer", TimeSpan.FromHours(1));
        A.CallTo(() => _authServiceMock.RefreshTokenAsync(A<CancellationToken>._)).Returns(Task.FromResult<TokenModel?>(validToken));

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        SpotifyRepository repository = new SpotifyRepository(_httpClientFactoryMock, _authServiceFactoryMock, _optionsMock, _loggerMock);

        List<HttpServiceResult<DtoSpotifyAlbum>> results = new List<HttpServiceResult<DtoSpotifyAlbum>>();

        // Act
        await foreach (HttpServiceResult<DtoSpotifyAlbum> result in repository.GetNewReleasesViaSearch("US", CancellationToken.None))
        {
            results.Add(result);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetNewReleasesViaSearch_ShouldSkip_NullAlbums()
    {
        // Arrange
        List<DtoSpotifyAlbum?> mixedAlbums = new List<DtoSpotifyAlbum?>
        {
            CreateTestSpotifyAlbums().First(),
            null,
            CreateTestSpotifyAlbums().Last()
        };
        object albumsResponse = CreateSpotifyAlbumsResponse(mixedAlbums.Where(x => x != null).Cast<DtoSpotifyAlbum>());
        string jsonResponse = JsonSerializer.Serialize(albumsResponse);

        TokenModel validToken = new TokenModel("valid_access_token", "Bearer", TimeSpan.FromHours(1));
        A.CallTo(() => _authServiceMock.RefreshTokenAsync(A<CancellationToken>._)).Returns(Task.FromResult<TokenModel?>(validToken));

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        SpotifyRepository repository = new SpotifyRepository(_httpClientFactoryMock, _authServiceFactoryMock, _optionsMock, _loggerMock);

        List<HttpServiceResult<DtoSpotifyAlbum>> results = new List<HttpServiceResult<DtoSpotifyAlbum>>();

        // Act
        await foreach (HttpServiceResult<DtoSpotifyAlbum> result in repository.GetNewReleasesViaSearch("US", CancellationToken.None))
        {
            results.Add(result);
        }

        // Assert
        results.Should().NotBeNull();
        results.Count.Should().Be(2);
        results.All(r => r.Content != null).Should().BeTrue();
    }

    [TestMethod]
    public async Task GetNewReleasesViaSearch_ShouldThrow_WhenAuthenticationFails()
    {
        // Arrange
        A.CallTo(() => _authServiceMock.RefreshTokenAsync(A<CancellationToken>._)).Returns(Task.FromResult<TokenModel?>(null));

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, "{}");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        SpotifyRepository repository = new SpotifyRepository(_httpClientFactoryMock, _authServiceFactoryMock, _optionsMock, _loggerMock);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<MissingTokenException>(async () =>
        {
            await foreach (HttpServiceResult<DtoSpotifyAlbum> result in repository.GetNewReleasesViaSearch("US", CancellationToken.None))
            {
                Assert.Fail("Expected MissingTokenException, but got result.");
            }
        });
    }

    [TestMethod]
    public async Task GetNewReleasesViaSearch_ShouldHandle_InvalidJsonResponse()
    {
        // Arrange
        TokenModel validToken = new TokenModel("valid_access_token", "Bearer", TimeSpan.FromHours(1));
        A.CallTo(() => _authServiceMock.RefreshTokenAsync(A<CancellationToken>._)).Returns(Task.FromResult<TokenModel?>(validToken));

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, "{ invalid: json }");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        SpotifyRepository repository = new SpotifyRepository(_httpClientFactoryMock, _authServiceFactoryMock, _optionsMock, _loggerMock);

        List<HttpServiceResult<DtoSpotifyAlbum>> results = [];

        // Act
        await foreach (HttpServiceResult<DtoSpotifyAlbum> result in repository.GetNewReleasesViaSearch("US", CancellationToken.None))
        {
            results.Add(result);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().BeEmpty(); 
    }

    [TestMethod]
    public async Task GetNewReleasesViaSearch_ShouldHandle_HttpErrorResponse()
    {
        // Arrange
        TokenModel validToken = new TokenModel("valid_access_token", "Bearer", TimeSpan.FromHours(1));
        A.CallTo(() => _authServiceMock.RefreshTokenAsync(A<CancellationToken>._)).Returns(Task.FromResult<TokenModel?>(validToken));

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.BadRequest, "Bad Request");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        SpotifyRepository repository = new SpotifyRepository(_httpClientFactoryMock, _authServiceFactoryMock, _optionsMock, _loggerMock);

        List<HttpServiceResult<DtoSpotifyAlbum>> results = new List<HttpServiceResult<DtoSpotifyAlbum>>();

        // Act
        await foreach (HttpServiceResult<DtoSpotifyAlbum> result in repository.GetNewReleasesViaSearch("US", CancellationToken.None))
        {
            results.Add(result);
        }

        // Assert
        results.Should().NotBeNull();
        results.Should().BeEmpty(); // HTTP error should result in no albums
    }


    [TestMethod]
    public void Constructor_ShouldCreateRepository_WhenValidOptions()
    {
        // Arrange
        HttpClient httpClient = new HttpClient();
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        // Act
        SpotifyRepository repository = new SpotifyRepository(_httpClientFactoryMock, _authServiceFactoryMock, _optionsMock, _loggerMock);

        // Assert
        repository.Should().NotBeNull();
        A.CallTo(() => _authServiceFactoryMock.CreateSpotifyAuthService()).MustHaveHappenedOnceExactly();
    }

    private HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string content)
    {
        MockHttpMessageHandler handler = new MockHttpMessageHandler(statusCode, content);
        return new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.spotify.com/")
        };
    }

    private IEnumerable<DtoSpotifyAlbum> CreateTestSpotifyAlbums()
    {
        return new List<DtoSpotifyAlbum>
        {
            new DtoSpotifyAlbum
            {
                Id = "4aawyAB9vmqN3hguQ7FjRGTy",
                Name = "NameAlbum1",
                AlbumType = "album",
                Href = "https://api.spotify.com/v1/albums/4aawyAB9vmqN3hguQ7FjRGTy",
                ReleaseDate = "2024-01-15",
                ReleaseDatePrecision = "day",
                TotalTracks = 12,
                Type = "album",
                Uri = "spotify:album:4aawyAB9vmqN3hguQ7FjRGTy",
                Artists = new List<DtoSpotifyArtist>
                {
                    new DtoSpotifyArtist
                    {
                        Id = "06HL4z0CvFAxychg27GXpf02",
                        Name = "Artist1",
                        Href = "https://api.spotify.com/v1/artists/06HL4z0CvFAxychg27GXpf02",
                        Type = "artist",
                        Uri = "spotify:artist:06HL4z0CvFAxychg27GXpf02"
                    }
                }
            },
            new DtoSpotifyAlbum
            {
                Id = "5Z9iiGl2FcIfagf3BMiv6OIw",
                Name = "NameAlbum2",
                AlbumType = "album",
                Href = "https://api.spotify.com/v1/albums/5Z9iiGl2FcIfagf3BMiv6OIw",
                ReleaseDate = "2024-02-20",
                ReleaseDatePrecision = "day",
                TotalTracks = 14,
                Type = "album",
                Uri = "spotify:album:5Z9iiGl2FcIfagf3BMiv6OIw",
                Artists = new List<DtoSpotifyArtist>
                {
                    new DtoSpotifyArtist
                    {
                        Id = "1Xyo4u8uXC1ZmMpatFdf05PJ",
                        Name = "Artist2",
                        Href = "https://api.spotify.com/v1/artists/1Xyo4u8uXC1ZmMpatFdf05PJ",
                        Type = "artist",
                        Uri = "spotify:artist:1Xyo4u8uXC1ZmMpatFdf05PJ"
                    }
                }
            }
        };
    }

    private object CreateSpotifyAlbumsResponse(IEnumerable<DtoSpotifyAlbum> albums)
    {
        return new
        {
            albums = new
            {
                href = "https://api.spotify.com/v1/search?query=year%3A2024%2Btag%3Anew&type=album&market=US&limit=50&offset=0",
                items = albums,
                limit = 50,
                next = (string)null,
                offset = 1000,
                previous = (string)null,
                total = albums.Count()
            }
        };
    }
}