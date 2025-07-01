using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
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
        private Mock<IDiscogRepository> _discogsServiceMock;
        private Mock<ISpotifyRepository> _spotifyServiceMock;
        private Mock<IReleaseAlbumRepository> _releaseAlbumRepositoryMock;
        private Mock<ILogger> _loggerMock;
        private Mock<IResult> _positiveResultMock; //=> in framework

        [TestInitialize]
        public void Initialize()
        {
            _discogsServiceMock = new Mock<IDiscogRepository>();
            _spotifyServiceMock = new Mock<ISpotifyRepository>();
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
              .ReturnsAsync(new Repositories.HttpServiceResult<IAuthor>(new Mock<IAuthor>().Object, true));
            _discogsServiceMock.Setup(x => x.GetLastReleaseByArtistID(It.IsAny<string>(), new CancellationToken()))
             .ReturnsAsync(new Repositories.HttpServiceResult<AlbumModel>(new Mock<AlbumModel>().Object, true));
            Dto.Spotify.DtoSpotifyAlbum release = new()
            {
                ReleaseDate = DateTime.UtcNow.AddDays(-15).ToString(),
                Artists =
                [
                    new Dto.Spotify.DtoSpotifyArtist()
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
            _releaseAlbumRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<AlbumModel>(), new CancellationToken()), Times.Never);
        }

        [TestMethod]
        public async Task FetchVinyl_Should_Continue_When_Realease_Is_NotTooOld()
        {
            //Arrange
            _discogsServiceMock.Setup(x => x.GetArtist(It.IsAny<string>(), new CancellationToken()))
                .ReturnsAsync(new Repositories.HttpServiceResult<IAuthor>(new Mock<IAuthor>().Object, true));
            _discogsServiceMock.Setup(x => x.GetLastReleaseByArtistID(It.IsAny<string>(), new CancellationToken()))
             .ReturnsAsync(new Repositories.HttpServiceResult<AlbumModel>(new Mock<AlbumModel>().Object, true));
            Dto.Spotify.DtoSpotifyAlbum release = new()
            {
                ReleaseDate = DateTime.UtcNow.AddDays(-5).ToString(),
                Artists =
                [
                    new Dto.Spotify.DtoSpotifyArtist()
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
            _releaseAlbumRepositoryMock.Verify(x => x.InsertAsync(It.IsAny<AlbumModel>(), new CancellationToken()), Times.Once);
        }
    }
}
