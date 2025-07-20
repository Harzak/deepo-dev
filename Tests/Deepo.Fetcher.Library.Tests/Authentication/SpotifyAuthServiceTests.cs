using Deepo.Fetcher.Library.Authentication;
using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Tests.TestUtils;
using Deepo.Framework.Results;
using Deepo.Framework.Web.Model;
using Deepo.Framework.Web.Service;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Tests.Authentication;

[TestClass]
public class SpotifyAuthServiceTests
{
    private IHttpClientFactory _httpClientFactoryMock;
    private IOptions<HttpServicesOption> _optionsMock;
    private ILogger<HttpService> _loggerMock;
    private HttpServicesOption _httpServicesOption;
    private HttpServiceOption _spotifyOption;
    private HttpServiceOption _spotifyAuthOption;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientFactoryMock = A.Fake<IHttpClientFactory>();
        _optionsMock = A.Fake<IOptions<HttpServicesOption>>();
        _loggerMock = A.Fake<ILogger<HttpService>>();

        _spotifyOption = new HttpServiceOption
        {
            Name = "Spotify",
            ConsumerKey = "test_consumer_key",
            ConsumerSecret = "test_consumer_secret",
            BaseAddress = new Uri("https://api.spotify.com/v1/"),
            UserAgent = "DeepoCatalog/1.0",
            TaskID = "spotify-task"
        };

        _spotifyAuthOption = new HttpServiceOption
        {
            Name = "SpotifyAuth",
            BaseAddress = new Uri("https://accounts.spotify.com/"),
            UserAgent = "DeepoCatalog/1.0",
            TaskID = "spotify-auth-task"
        };

        _httpServicesOption = new HttpServicesOption
        {
            Spotify = _spotifyOption,
            SpotifyAuth = _spotifyAuthOption
        };

        A.CallTo(() => _optionsMock.Value).Returns(_httpServicesOption);
    }

    [TestMethod]
    public async Task RefreshTokenAsync_ShouldReturn_ValidToken_WhenSuccessfulResponse()
    {
        // Arrange
        var expectedToken = new SpotifyToken
        {
            AccessToken = "BQDKxZ9qZfGJ2K...",
            TokenType = "Bearer",
            ExpiresIn = 3600
        };

        string jsonResponse = JsonSerializer.Serialize(expectedToken);
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var service = new SpotifyAuthService(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        TokenModel result = await service.RefreshTokenAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(expectedToken.AccessToken);
        result.Type.Should().Be(expectedToken.TokenType);
        result.IsExpired.Should().BeFalse();
    }

    [TestMethod]
    public async Task RefreshTokenAsync_ShouldReturn_Null_WhenUnsuccessfulResponse()
    {
        // Arrange
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.Unauthorized, "Unauthorized");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var service = new SpotifyAuthService(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        TokenModel result = await service.RefreshTokenAsync(CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task RefreshTokenAsync_ShouldReturn_Null_WhenEmptyResponse()
    {
        // Arrange
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, string.Empty);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var service = new SpotifyAuthService(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        TokenModel result = await service.RefreshTokenAsync(CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task RefreshTokenAsync_ShouldReturn_Null_WhenNullAccessToken()
    {
        // Arrange
        var invalidToken = new SpotifyToken
        {
            AccessToken = null,
            TokenType = "Bearer",
            ExpiresIn = 3600
        };

        string jsonResponse = JsonSerializer.Serialize(invalidToken);
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var service = new SpotifyAuthService(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        TokenModel result = await service.RefreshTokenAsync(CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public async Task RefreshTokenAsync_ShouldReturn_Null_WhenNullTokenType()
    {
        // Arrange
        var invalidToken = new SpotifyToken
        {
            AccessToken = "BQDKxZ9qZfGJ2K...",
            TokenType = null,
            ExpiresIn = 3600
        };

        string jsonResponse = JsonSerializer.Serialize(invalidToken);
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var service = new SpotifyAuthService(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        TokenModel result = await service.RefreshTokenAsync(CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [TestMethod]
    public void Constructor_ShouldSetBasicAuthorization_WhenValidOptions()
    {
        // Arrange
        var httpClient = new HttpClient();
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        // Act
        var service = new SpotifyAuthService(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Assert
        string expectedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_spotifyOption.ConsumerKey}:{_spotifyOption.ConsumerSecret}"));
        
        httpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
        httpClient.DefaultRequestHeaders.Authorization.Scheme.Should().Be("Basic");
        httpClient.DefaultRequestHeaders.Authorization.Parameter.Should().Be(expectedCredentials);
    }

    private static HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string content)
    {
        var handler = new MockHttpMessageHandler(statusCode, content);
        return new HttpClient(handler)
        {
            BaseAddress = new Uri("https://accounts.spotify.com/")
        };
    }
}