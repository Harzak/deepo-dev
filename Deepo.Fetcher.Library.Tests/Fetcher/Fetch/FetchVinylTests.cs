using Deepo.DAL.Service.Features.Author;
using Deepo.DAL.Service.Features.ReleaseAlbum;
using Deepo.Fetcher.Library.Fetcher.Fetch;
using Deepo.Fetcher.Library.Service.Discogs;
using Deepo.Fetcher.Library.Service.Spotify;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
        private Mock<IReleaseAlbumDBService> _releaseAlbumDBServiceMock;
        private Mock<ILogger> _loggerMock;
        private Mock<IResult> _positiveResultMock; //=> in framework

        [TestInitialize]
        public void Initialize()
        {
            _discogsServiceMock = new Mock<IDiscogService>();
            _spotifyServiceMock = new Mock<ISpotifyService>();
            _releaseAlbumDBServiceMock = new Mock<IReleaseAlbumDBService>();
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
             .ReturnsAsync(new Service.HttpServiceResult<IAlbum>(new Mock<IAlbum>().Object, true));
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
            FetchVinyl fetch = new(release, _discogsServiceMock.Object, _spotifyServiceMock.Object, _releaseAlbumDBServiceMock.Object, _loggerMock.Object)
            {
                //Act
                MinReleaseDate = DateTime.UtcNow.AddDays(-15)
            };
            await fetch.StartAsync(new CancellationToken());

            //Assert
            _releaseAlbumDBServiceMock.Verify(x => x.Insert(It.IsAny<IAlbum>(), new CancellationToken()), Times.Never);
        }

        [TestMethod]
        public async Task FetchVinyl_Should_Continue_When_Realease_Is_NotTooOld()
        {
            //Arrange
            _discogsServiceMock.Setup(x => x.GetArtist(It.IsAny<string>(), new CancellationToken()))
                .ReturnsAsync(new Service.HttpServiceResult<IAuthor>(new Mock<IAuthor>().Object, true));
            _discogsServiceMock.Setup(x => x.GetLastReleaseByArtistID(It.IsAny<string>(), new CancellationToken()))
             .ReturnsAsync(new Service.HttpServiceResult<IAlbum>(new Mock<IAlbum>().Object, true));
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
            FetchVinyl fetch = new(release, _discogsServiceMock.Object, _spotifyServiceMock.Object, _releaseAlbumDBServiceMock.Object, _loggerMock.Object)
            {
                //Act

                MinReleaseDate = DateTime.UtcNow.AddDays(-15)
            };
            await fetch.StartAsync(new CancellationToken());

            //Assert
            _releaseAlbumDBServiceMock.Verify(x => x.Insert(It.IsAny<IAlbum>(), new CancellationToken()), Times.Once);
        }
    }
}
