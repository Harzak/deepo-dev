using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Fetcher.Fetch;
using Deepo.Fetcher.Library.Interfaces;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Tests.Fetcher.Fetch
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FetchVinylTests
    {
        private Mock<IDiscogService> _discogsServiceMock;
        private Mock<ISpotifyService> _spotifyServiceMock;
        private Mock<IReleaseAlbumRepository> _releaseAlbumRepositoryMock;
        private Mock<ILogger> _loggerMock;
        private Mock<IResult> _positiveResultMock; //=> in framework

        [TestInitialize]
        public void Initialize()
        {
            _discogsServiceMock = new Mock<IDiscogService>();
            _spotifyServiceMock = new Mock<ISpotifyService>();
            _releaseAlbumRepositoryMock = new Mock<IReleaseAlbumRepository>();
            _positiveResultMock = new Mock<IResult>();
            _positiveResultMock.Setup(x => x.IsSuccess).Returns(true);
            _loggerMock = new Mock<ILogger>();
        }

        [TestMethod]
        public async Task FetchVinyl_ShouldNot_Continue_When_Realease_Is_TooOld()
        {
            //Arrange
            _discogsServiceMock.Setup(x => x.GetArtist(It.IsAny<string>(), new CancellationToken()))
              .ReturnsAsync(new Service.HttpServiceResult<IAuthor>(new Mock<IAuthor>().Object, true));
            _discogsServiceMock.Setup(x => x.GetLastReleaseByArtistID(It.IsAny<string>(), new CancellationToken()))
             .ReturnsAsync(new Service.HttpServiceResult<AlbumModel>(new Mock<AlbumModel>().Object, true));
            Dto.Spotify.Album release = new()
            {
                ReleaseDate = DateTime.UtcNow.AddDays(-15).ToString(),
                Artists =
                [
                    new Dto.Spotify.Artist()
                    {
                         Name = "Darude"
                    }
                ],
                Name = "Sandstorm"
            };
            FetchVinyl fetch = new(release, _discogsServiceMock.Object, _spotifyServiceMock.Object, _releaseAlbumRepositoryMock.Object, _loggerMock.Object)
            {
                //Act
                MinReleaseDate = DateTime.UtcNow.AddDays(-15)
            };
            await fetch.StartAsync(new CancellationToken());

            //Assert
            _releaseAlbumRepositoryMock.Verify(x => x.Insert(It.IsAny<AlbumModel>(), new CancellationToken()), Times.Never);
        }

        [TestMethod]
        public async Task FetchVinyl_Should_Continue_When_Realease_Is_NotTooOld()
        {
            //Arrange
            _discogsServiceMock.Setup(x => x.GetArtist(It.IsAny<string>(), new CancellationToken()))
                .ReturnsAsync(new Service.HttpServiceResult<IAuthor>(new Mock<IAuthor>().Object, true));
            _discogsServiceMock.Setup(x => x.GetLastReleaseByArtistID(It.IsAny<string>(), new CancellationToken()))
             .ReturnsAsync(new Service.HttpServiceResult<AlbumModel>(new Mock<AlbumModel>().Object, true));
            Dto.Spotify.Album release = new()
            {
                ReleaseDate = DateTime.UtcNow.AddDays(-5).ToString(),
                Artists =
                [
                    new Dto.Spotify.Artist()
                    {
                         Name = "Darude"
                    }
                ],
                Name = "Sandstorm"
            };
            FetchVinyl fetch = new(release, _discogsServiceMock.Object, _spotifyServiceMock.Object, _releaseAlbumRepositoryMock.Object, _loggerMock.Object)
            {
                //Act

                MinReleaseDate = DateTime.UtcNow.AddDays(-15)
            };
            await fetch.StartAsync(new CancellationToken());

            //Assert
            _releaseAlbumRepositoryMock.Verify(x => x.Insert(It.IsAny<AlbumModel>(), new CancellationToken()), Times.Once);
        }
    }
}
