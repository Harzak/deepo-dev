using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Repositories.Discogs;
using Deepo.Fetcher.Library.Tests.TestUtils;
using Deepo.Framework.Results;
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
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Tests.Repositories.Discogs;

[TestClass]
public class DiscogRepositoryTests
{
    private IHttpClientFactory _httpClientFactoryMock;
    private IOptions<HttpServicesOption> _optionsMock;
    private ILogger<DiscogRepository> _loggerMock;
    private HttpServicesOption _httpServicesOption;
    private HttpServiceOption _discogsOption;

    [TestInitialize]
    public void Initialize()
    {
        _httpClientFactoryMock = A.Fake<IHttpClientFactory>();
        _optionsMock = A.Fake<IOptions<HttpServicesOption>>();
        _loggerMock = A.Fake<ILogger<DiscogRepository>>();

        _discogsOption = new HttpServiceOption
        {
            Name = "Discogs",
            BaseAddress = new Uri("https://api.discogs.com/"),
            UserAgent = "DeepoCatalog/1.0",
            TaskID = "discogs-task",
            Token = "test_discogs_token"
        };

        _httpServicesOption = new HttpServicesOption
        {
            Discogs = _discogsOption
        };

        A.CallTo(() => _optionsMock.Value).Returns(_httpServicesOption);
    }

    [TestMethod]
    public async Task GetSearchByReleaseTitleAndYear_ShouldReturn_Success_WhenValidResponse()
    {
        // Arrange
        IEnumerable<DtoDiscogsAlbum> expectedAlbums = CreateTestAlbums();
        object searchResponse = CreateAlbumSearchResponse(expectedAlbums);
        string jsonResponse = JsonSerializer.Serialize(searchResponse);

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = await repository.GetSearchByReleaseTitleAndYear("TestAlbum", 2024, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.Count().Should().Be(expectedAlbums.Count());
        result.Content.First().Title.Should().Be(expectedAlbums.First().Title);
        result.Content.First().Id.Should().Be(expectedAlbums.First().Id);
    }

    [TestMethod]
    public async Task GetSearchByReleaseTitleAndYear_ShouldReturn_Error_WhenHttpRequestFails()
    {
        // Arrange
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.BadRequest, "Bad Request");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = await repository.GetSearchByReleaseTitleAndYear("Test Album", 2024, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetSearchByReleaseTitleAndYear_ShouldReturn_Error_WhenParsingFails()
    {
        // Arrange
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, "{ invalid json }");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = await repository.GetSearchByReleaseTitleAndYear("Test Album", 2024, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetSearchByReleaseTitleAndYear_ShouldReturn_Success_WhenEmptyResults()
    {
        // Arrange
        object searchResponse = CreateAlbumSearchResponse(new List<DtoDiscogsAlbum>());
        string jsonResponse = JsonSerializer.Serialize(searchResponse);

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = await repository.GetSearchByReleaseTitleAndYear("UnknownAlbum", 2024, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GetSearchByArtistNameAndYear_ShouldReturn_Success_WhenValidResponse()
    {
        // Arrange
        IEnumerable<DtoDiscogsAlbum> expectedAlbums = CreateTestAlbums();
        object searchResponse = CreateAlbumSearchResponse(expectedAlbums);
        string jsonResponse = JsonSerializer.Serialize(searchResponse);

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = await repository.GetSearchByArtistNameAndYear("TestArtist", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.Count().Should().Be(expectedAlbums.Count());
        result.Content.First().Title.Should().Be(expectedAlbums.First().Title);
    }

    [TestMethod]
    public async Task GetSearchByArtistNameAndYear_ShouldReturn_Error_WhenHttpRequestFails()
    {
        // Arrange
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.NotFound, "Not Found");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = await repository.GetSearchByArtistNameAndYear("Unknown Artist", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetSearchByArtistNameAndYear_ShouldReturn_Error_WhenParsingFails()
    {
        // Arrange
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, "{ malformed: json }");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = await repository.GetSearchByArtistNameAndYear("TestArtist", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetReleaseByID_ShouldReturn_Success_WhenValidResponse()
    {
        // Arrange
        DtoDiscogsRelease expectedRelease = CreateTestRelease();
        string jsonResponse = JsonSerializer.Serialize(expectedRelease);

        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, jsonResponse);
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<DtoDiscogsRelease> result = await repository.GetReleaseByID("123456", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().NotBeNull();
        result.Content.Id.Should().Be(expectedRelease.Id);
        result.Content.Title.Should().Be(expectedRelease.Title);
        result.Content.Year.Should().Be(expectedRelease.Year);
    }

    [TestMethod]
    public async Task GetReleaseByID_ShouldReturn_Error_WhenHttpRequestFails()
    {
        // Arrange
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.Unauthorized, "Unauthorized");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<DtoDiscogsRelease> result = await repository.GetReleaseByID("123456", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task GetReleaseByID_ShouldReturn_Error_WhenParsingFails()
    {
        // Arrange
        HttpClient httpClient = CreateMockHttpClient(HttpStatusCode.OK, "{ invalid: structure }");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<DtoDiscogsRelease> result = await repository.GetReleaseByID("123456", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetReleaseByID_ShouldReturn_Error_WhenNullResponse()
    {
        // Arrange
        HttpClient   httpClient = CreateMockHttpClient(HttpStatusCode.OK, "null");
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Act
        OperationResult<DtoDiscogsRelease> result = await repository.GetReleaseByID("123456", CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().Be("Failed to parse Discogs release return data");
    }

    [TestMethod]
    public void Constructor_ShouldSetDiscogsAuthorization_WhenValidOptions()
    {
        // Arrange
        HttpClient  httpClient = new HttpClient();
        A.CallTo(() => _httpClientFactoryMock.CreateClient(A<string>._)).Returns(httpClient);

        // Act
        var repository = new DiscogRepository(_httpClientFactoryMock, _optionsMock, _loggerMock);

        // Assert
        httpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
        httpClient.DefaultRequestHeaders.Authorization.Scheme.Should().Be("Discogs");
        httpClient.DefaultRequestHeaders.Authorization.Parameter.Should().Be($"token={_discogsOption.Token}");
    }

    private static HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string content)
    {
        var handler = new MockHttpMessageHandler(statusCode, content);
        return new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.discogs.com/")
        };
    }

    private IEnumerable<DtoDiscogsAlbum> CreateTestAlbums()
    {
        return new List<DtoDiscogsAlbum>
        {
            new DtoDiscogsAlbum
            {
                Id = 123456,
                Title = "Test Album",
                Year = "2024",
                Country = "US",
                Genre = new[] { "Rock", "Alternative" },
                Style = new[] { "Indie Rock" },
                Format = new[] { "Vinyl", "LP" },
                Label = new[] { "Test Records" },
                CoverImage = "https://example.com/cover.jpg",
                Thumb = "https://example.com/thumb.jpg",
                MasterId = 789,
                Uri = "/releases/123456"
            },
            new DtoDiscogsAlbum
            {
                Id = 654321,
                Title = "Another Test Album",
                Year = "2024",
                Country = "UK",
                Genre = new[] { "Electronic" },
                Style = new[] { "House" },
                Format = new[] { "Vinyl", "12\"" },
                Label = new[] { "Electronic Records" },
                CoverImage = "https://example.com/cover2.jpg",
                Thumb = "https://example.com/thumb2.jpg",
                MasterId = 987,
                Uri = "/releases/654321"
            }
        };
    }

    private DtoDiscogsRelease CreateTestRelease()
    {
        return new DtoDiscogsRelease
        {
            Id = 123456,
            Title = "Test Release",
            Year = 2024,
            Country = "US",
            Status = "Accepted",
            DataQuality = "Correct",
            Released = "2024-01-15",
            ReleasedFormatted = "15 Jan 2024",
            Genres = new[] { "Rock", "Alternative" },
            Styles = new[] { "Indie Rock" },
            ThumbsURL = "https://example.com/thumb.jpg",
            Uri = "/releases/123456",
            ResourceUrl = "https://api.discogs.com/releases/123456",
            DateAdded = DateTime.UtcNow.AddDays(-30),
            DateChanged = DateTime.UtcNow.AddDays(-1)
        };
    }

    private object CreateAlbumSearchResponse(IEnumerable<DtoDiscogsAlbum> albums)
    {
        return new
        {
            results = albums,
            pagination = new
            {
                per_page = 50,
                pages = 1,
                page = 1,
                urls = new { }
            }
        };
    }
}