using Deepo.DAL.Service.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Deepo.Mediator.Tests.Handler
{
    [TestClass]
    public class GetVinylReleaseHandlerTests
    {
        private Mock<IReleaseAlbumDBService> _dbMock;

        [TestInitialize]
        public void Initialize()
        {
            _dbMock = new Mock<IReleaseAlbumDBService>();
        }

    }
}
