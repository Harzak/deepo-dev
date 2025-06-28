using Deepo.DAL.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Deepo.Mediator.Tests.Handler
{
    [TestClass]
    public class GetVinylReleaseHandlerTests
    {
        private Mock<IReleaseAlbumRepository> _dbMock;

        [TestInitialize]
        public void Initialize()
        {
            _dbMock = new Mock<IReleaseAlbumRepository>();
        }

    }
}
