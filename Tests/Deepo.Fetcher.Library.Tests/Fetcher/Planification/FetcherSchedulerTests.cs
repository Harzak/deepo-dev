
using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Fetcher.Planification;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace Deepo.Fetcher.Library.Tests.Fetchers.Planification
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class FetcherSchedulerTests
    {
        private readonly FetchersScheduler? _scheduler;
        private readonly ILogger<FetchersScheduler>? _logger;
        private readonly Mock<ITimer> _timer;
        private readonly Mock<IPlanificationRepository> _planificationDBServiceMock;

        //[TestInitialize]
        //public void Initialize()
        //{
        //    _logger = new Mock<ILogger<FetchersScheduler>>().Object;
        //    _timer = new Mock<ITimer>();
        //    _planificationDBServiceMock = new Mock<IPlanificationDBService>();
        //    _scheduler = new FetchersScheduler(_planificationDBServiceMock.Object,
        //        _timer.Object, 
        //        new Framework.Common.Utils.Time.Provider.TimeProvider(),
        //        _logger);
        //}

        //[TestMethod]
        //public void OneShotWorkerShouldStart()
        //{
        //    //Arrange
        //    Mock<IWorker> workerMock = new();
        //    workerMock.Setup(x => x.ID).Returns(Guid.NewGuid());
        //    OneShotPlanning workerPlanningDb = new(new TimeProvider(), _logger);
        //    _planificationDBServiceMock.Setup(x => x.GetPlanification(workerMock.Object)).Returns(workerPlanningDb);
        //    List<IWorker> readyWorkers = new();
        //    _scheduler.ReadyToStart += delegate (object? sender, WorkerEventArgs e)
        //    {
        //        readyWorkers.Add(e.Worker);
        //    };

        //    //Act
        //    _scheduler.RegisterOneShot(workerMock.Object);

        //    //Assert
        //    readyWorkers.Should().Contain(workerMock.Object);
        //}

        //[TestMethod]
        //public async Task OneShotWorkerShouldNotRestart()
        //{
        //    //Arrange
        //    Mock<IWorker> worker = new Mock<IWorker>();
        //    worker.Setup(x => x.ID).Returns(Guid.NewGuid());

        //    _scheduler.RegisterOneShot(worker.Object);
        //    List<IWorker> readyWorkers = new();
        //    _scheduler.ReadyToStart += delegate (object? sender, WorkerEventArgs e)
        //    {
        //        readyWorkers.Add(e.Worker);
        //    };

        //    //Act
        //    await worker.Object.StartAsync(new CancellationToken()).ConfigureAwait(true);
        //    EvaluateReadyWorker();

        //    //Assert
        //    readyWorkers.Should().NotContain(worker.Object);
        //}


        //private void EvaluateReadyWorker()
        //{
        //    _timer.Raise(x => x.Elapsed -= null, new EventArgs() as ElapsedEventArgs);
        //}
    }
}
