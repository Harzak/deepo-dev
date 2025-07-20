using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Strategies.Vinyl;
using Deepo.Framework.Results;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Tests.Strategies.Vinyl;

[TestClass]
public class DiscogsSearchByArtistsStrategyTests
{
    private IDiscogRepository _discogRepositoryMock;
    private ILogger _loggerMock;
    private DiscogsSearchByArtistsStrategy _strategy;

    [TestInitialize]
    public void Initialize()
    {
        _discogRepositoryMock = A.Fake<IDiscogRepository>();
        _loggerMock = A.Fake<ILogger>();
        _strategy = new DiscogsSearchByArtistsStrategy(_discogRepositoryMock, _loggerMock);
    }

    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [TestMethod]
    public async Task SearchAsync_ShouldReturnError_WhenNoArtistName(string name)
    {
        // Act
        OperationResultList<DtoDiscogsRelease> result = await _strategy.SearchAsync(name, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task SearchAsync_ShouldReturnError_WhenDiscogRepositoryFails()
    {
        // Arrange
        string artistName = "TestArtist";
        OperationResult<IEnumerable<DtoDiscogsAlbum>> failedResult = new OperationResult<IEnumerable<DtoDiscogsAlbum>>()
            .WithFailure();

        A.CallTo(() => _discogRepositoryMock.GetSearchByArtistNameAndYear(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult(failedResult));

        // Act
        OperationResultList<DtoDiscogsRelease> result = await _strategy.SearchAsync(artistName, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.HasContent.Should().BeFalse();

        A.CallTo(() => _discogRepositoryMock.GetSearchByArtistNameAndYear(A<string>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task SearchAsync_ShouldReturnSuccess_WhenRecentReleasesFound()
    {
        // Arrange
        string artistName = "TestArtist";
        List<DtoDiscogsAlbum> albums = CreateTestDiscogsAlbums();
        List<DtoDiscogsRelease> recentReleases = CreateTestDiscogsReleases(recentDatesOnly: true);

        SetupSuccessfulAlbumSearch(albums);
        SetupSuccessfulReleaseDetails(recentReleases);

        // Act
        OperationResultList<DtoDiscogsRelease> result = await _strategy.SearchAsync(artistName, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().HaveCount(2);
        result.Content.Should().OnlyContain(r => r.Title != null);

        A.CallTo(() => _discogRepositoryMock.GetSearchByArtistNameAndYear(A<string>._, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _discogRepositoryMock.GetReleaseByID(A<string>._, A<CancellationToken>._))
            .MustHaveHappened(albums.Count, Times.Exactly);
    }

    [TestMethod]
    public async Task SearchAsync_ShouldFilterOutOldReleases_WhenReleasesAreOlderThan50Days()
    {
        // Arrange
        string artistName = "TestArtist";
        List<DtoDiscogsAlbum> albums = CreateTestDiscogsAlbums();
        List<DtoDiscogsRelease> oldReleases = CreateTestDiscogsReleases(recentDatesOnly: false);

        SetupSuccessfulAlbumSearch(albums);
        SetupSuccessfulReleaseDetails(oldReleases);

        // Act
        OperationResultList<DtoDiscogsRelease> result = await _strategy.SearchAsync(artistName, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.HasContent.Should().BeFalse();
    }

    [TestMethod]
    public async Task SearchAsync_ShouldHandleInvalidReleaseDates_AndContinueProcessing()
    {
        // Arrange
        string artistName = "TestArtist";
        List<DtoDiscogsAlbum> albums = CreateTestDiscogsAlbums();
        List<DtoDiscogsRelease> releasesWithInvalidDates =
        [
            new DtoDiscogsRelease
            {
                Id = 1,
                Title = "Release with Invalid Date",
                Released = "invalid-date"
            },
            new DtoDiscogsRelease
            {
                Id = 2,
                Title = "Release with Recent Date",
                Released = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd")
            }
        ];

        SetupSuccessfulAlbumSearch(albums);
        SetupSuccessfulReleaseDetails(releasesWithInvalidDates);

        // Act
        OperationResultList<DtoDiscogsRelease> result = await _strategy.SearchAsync(artistName, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().HaveCount(1);
        result.Content.First().Title.Should().Be("Release with Recent Date");
    }

    [TestMethod]
    public async Task SearchAsync_ShouldSkipNullOrZeroIdAlbums_WhenFilteringResults()
    {
        // Arrange
        string artistName = "TestArtist";
        List<DtoDiscogsAlbum> albumsWithNullAndZero =
        [
            new DtoDiscogsAlbum { Id = 1, Title = "Valid Album 1" },
            new DtoDiscogsAlbum { Id = 0, Title = "Invalid Album with Zero ID" },
            new DtoDiscogsAlbum { Id = 2, Title = "Valid Album 2" },
            null
        ];

        List<DtoDiscogsRelease> releases = CreateTestDiscogsReleases(recentDatesOnly: true);

        SetupSuccessfulAlbumSearch(albumsWithNullAndZero);
        SetupSuccessfulReleaseDetails(releases);

        // Act
        OperationResultList<DtoDiscogsRelease> result = await _strategy.SearchAsync(artistName, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().HaveCount(2);

        A.CallTo(() => _discogRepositoryMock.GetReleaseByID("1", A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _discogRepositoryMock.GetReleaseByID("2", A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => _discogRepositoryMock.GetReleaseByID("0", A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [TestMethod]
    public async Task SearchAsync_ShouldContinueProcessing_WhenSomeReleaseDetailsRequestsFail()
    {
        // Arrange
        string artistName = "TestArtist";
        List<DtoDiscogsAlbum> albums = CreateTestDiscogsAlbums();
        DtoDiscogsRelease successfulRelease = new DtoDiscogsRelease
        {
            Id = 2,
            Title = "Successful Release",
            Released = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd")
        };

        SetupSuccessfulAlbumSearch(albums);

        // Setup first release request to fail, second to succeed
        A.CallTo(() => _discogRepositoryMock.GetReleaseByID("1", A<CancellationToken>._))
            .Returns(Task.FromResult(new OperationResult<DtoDiscogsRelease>().WithFailure().WithError("Release not found")));

        A.CallTo(() => _discogRepositoryMock.GetReleaseByID("2", A<CancellationToken>._))
            .Returns(Task.FromResult(new OperationResult<DtoDiscogsRelease>().WithSuccess().WithValue(successfulRelease)));

        // Act
        OperationResultList<DtoDiscogsRelease> result = await _strategy.SearchAsync(artistName, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.HasContent.Should().BeTrue();
        result.Content.Should().HaveCount(1);
        result.Content.First().Title.Should().Be("Successful Release");
    }

    [TestMethod]
    public async Task SearchAsync_ShouldUrlEncodeArtistName_WhenCallingRepository()
    {
        // Arrange
        string artistNameWithSpecialChars = "Test Artist & Band";
        string expectedEncodedName = "Test+Artist+%26+Band";
        List<DtoDiscogsAlbum> albums = CreateTestDiscogsAlbums();

        SetupSuccessfulAlbumSearch(albums);
        SetupSuccessfulReleaseDetails(CreateTestDiscogsReleases(recentDatesOnly: true));

        // Act
        await _strategy.SearchAsync(artistNameWithSpecialChars, CancellationToken.None);

        // Assert
        A.CallTo(() => _discogRepositoryMock.GetSearchByArtistNameAndYear(expectedEncodedName, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [TestMethod]
    public async Task SearchAsync_ShouldHandleEmptyAlbumList_WhenRepositoryReturnsNoResults()
    {
        // Arrange
        string artistName = "TestArtist";
        List<DtoDiscogsAlbum> emptyAlbums = [];

        SetupSuccessfulAlbumSearch(emptyAlbums);

        // Act
        OperationResultList<DtoDiscogsRelease> result = await _strategy.SearchAsync(artistName, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.ErrorMessage.Should().Contain("Search by artists failed, no results found this month");
        result.HasContent.Should().BeFalse();

        A.CallTo(() => _discogRepositoryMock.GetReleaseByID(A<string>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    private List<DtoDiscogsAlbum> CreateTestDiscogsAlbums()
    {
        return new List<DtoDiscogsAlbum>
        {
            new DtoDiscogsAlbum
            {
                Id = 1,
                Title = "Test Album 1",
                Year = "2024"
            },
            new DtoDiscogsAlbum
            {
                Id = 2,
                Title = "Test Album 2",
                Year = "2024"
            }
        };
    }

    private List<DtoDiscogsRelease> CreateTestDiscogsReleases(bool recentDatesOnly)
    {
        DateTime baseDate = recentDatesOnly ? DateTime.Now.AddDays(-10) : DateTime.Now.AddDays(-60);

        return new List<DtoDiscogsRelease>
        {
            new DtoDiscogsRelease
            {
                Id = 1,
                Title = "Test Release 1",
                Released = baseDate.ToString("yyyy-MM-dd"),
                Artists = new List<DtoDiscogsArtist>
                {
                    new DtoDiscogsArtist { Name = "Test Artist", Id = 1 }
                }
            },
            new DtoDiscogsRelease
            {
                Id = 2,
                Title = "Test Release 2",
                Released = baseDate.AddDays(5).ToString("yyyy-MM-dd"),
                Artists = new List<DtoDiscogsArtist>
                {
                    new DtoDiscogsArtist { Name = "Test Artist", Id = 1 }
                }
            }
        };
    }

    private void SetupSuccessfulAlbumSearch(List<DtoDiscogsAlbum> albums)
    {
        OperationResult<IEnumerable<DtoDiscogsAlbum>> successResult = new OperationResult<IEnumerable<DtoDiscogsAlbum>>()
            .WithSuccess()
            .WithValue(albums);

        A.CallTo(() => _discogRepositoryMock.GetSearchByArtistNameAndYear(A<string>._, A<CancellationToken>._))
            .Returns(Task.FromResult(successResult));
    }

    private void SetupSuccessfulReleaseDetails(List<DtoDiscogsRelease> releases)
    {
        foreach (DtoDiscogsRelease release in releases)
        {
            OperationResult<DtoDiscogsRelease> releaseResult = new OperationResult<DtoDiscogsRelease>()
                .WithSuccess()
                .WithValue(release);

            A.CallTo(() => _discogRepositoryMock.GetReleaseByID(release.Id.ToString(CultureInfo.CurrentCulture), A<CancellationToken>._))
                .Returns(Task.FromResult(releaseResult));
        }
    }
}