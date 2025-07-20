using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Workers.Scheduling;
using Deepo.Framework.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Tests.Workers.Scheduling;

[TestClass]
public class FetcherSchedulerDueEventTests
{
    private IDateTimeFacade _dateTimeFacadeMock;
    private ILogger _loggerMock;
    private DateTime _baseDateTimeUtc;
    private const string TestFetcherIdentifier = "test-fetcher";

    [TestInitialize]
    public void Initialize()
    {
        _dateTimeFacadeMock = A.Fake<IDateTimeFacade>();
        _loggerMock = A.Fake<ILogger>();
        _baseDateTimeUtc = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        
        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .Returns(_baseDateTimeUtc);
    }

    [TestMethod]
    public void Constructor_ShouldCreateDueEvent_WhenValidParameters()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);

        // Act
        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        // Assert
        dueEvent.Should().NotBeNull();
        dueEvent.FetcherIdentifier.Should().Be(TestFetcherIdentifier);
        dueEvent.EventIdentifier.Should().NotBe(Guid.Empty);
    }

    [TestMethod]
    public void Constructor_ShouldThrowArgumentException_WhenDueDateIsInPast()
    {
        // Arrange
        DateTime pastDueAt = _baseDateTimeUtc.AddHours(-1);

        // Act & Assert
        Action act = () => new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            pastDueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Due date must be in the future.*")
            .And.ParamName.Should().Be("dueAt");
    }

    [TestMethod]
    public void EventIdentifier_ShouldBeUnique_WhenMultipleInstancesCreated()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);

        // Act
        FetcherSchedulerDueEvent dueEvent1 = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        FetcherSchedulerDueEvent dueEvent2 = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        // Assert
        dueEvent1.EventIdentifier.Should().NotBe(dueEvent2.EventIdentifier);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldFireTriggeredEvent_WhenDueTimeReached()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddMilliseconds(50);
        bool triggeredEventFired = false;
        bool abortedEventFired = false;
        DueEventArgs? triggeredEventArgs = null;

        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        dueEvent.TriggeredEvent += (sender, args) =>
        {
            triggeredEventFired = true;
            triggeredEventArgs = args;
        };

        dueEvent.AbortedEvent += (sender, args) =>
        {
            abortedEventFired = true;
        };

        // Act
        await dueEvent.StartAsync(CancellationToken.None);
        await Task.Delay(100); 
        await dueEvent.StopAsync(CancellationToken.None);

        // Assert
        triggeredEventFired.Should().BeTrue();
        abortedEventFired.Should().BeFalse();
        triggeredEventArgs.Should().NotBeNull();
        triggeredEventArgs!.FetcherIdentifier.Should().Be(TestFetcherIdentifier);
        triggeredEventArgs.EventIdentifier.Should().Be(dueEvent.EventIdentifier);
        triggeredEventArgs.DueAt.Should().Be(dueAt);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldFireTriggeredEventImmediately_WhenDueTimeIsInPastDuringExecution()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);
        bool triggeredEventFired = false;
        DueEventArgs? triggeredEventArgs = null;

        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .ReturnsNextFromSequence(
                _baseDateTimeUtc,         
                dueAt.AddMinutes(1)       
            );

        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        dueEvent.TriggeredEvent += (sender, args) =>
        {
            triggeredEventFired = true;
            triggeredEventArgs = args;
        };

        // Act
        await dueEvent.StartAsync(CancellationToken.None);
        await Task.Delay(10); // Minimal delay to allow ExecuteAsync to start
        await dueEvent.StopAsync(CancellationToken.None);

        // Assert
        triggeredEventFired.Should().BeTrue();
        triggeredEventArgs.Should().NotBeNull();
        triggeredEventArgs!.FetcherIdentifier.Should().Be(TestFetcherIdentifier);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldFireAbortedEvent_WhenCancellationRequested()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);
        bool triggeredEventFired = false;
        bool abortedEventFired = false;
        DueEventArgs? abortedEventArgs = null;

        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .Returns(_baseDateTimeUtc); 

        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        dueEvent.TriggeredEvent += (sender, args) =>
        {
            triggeredEventFired = true;
        };

        dueEvent.AbortedEvent += (sender, args) =>
        {
            abortedEventFired = true;
            abortedEventArgs = args;
        };

        CancellationTokenSource cts = new CancellationTokenSource();

        // Act
        await dueEvent.StartAsync(cts.Token);
        cts.Cancel();
        await Task.Delay(10); 
        await dueEvent.StopAsync(CancellationToken.None);

        // Assert
        abortedEventFired.Should().BeTrue();
        triggeredEventFired.Should().BeFalse();
        abortedEventArgs.Should().NotBeNull();
        abortedEventArgs!.FetcherIdentifier.Should().Be(TestFetcherIdentifier);
        abortedEventArgs.EventIdentifier.Should().Be(dueEvent.EventIdentifier);
        abortedEventArgs.DueAt.Should().Be(dueAt);
    }

    [TestMethod]
    public async Task StopAsync_ShouldFireAbortedEvent_WhenStoppedBeforeDueTime()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);
        bool triggeredEventFired = false;
        bool abortedEventFired = false;
        DueEventArgs? abortedEventArgs = null;

        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .Returns(_baseDateTimeUtc); 

        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        dueEvent.TriggeredEvent += (sender, args) =>
        {
            triggeredEventFired = true;
        };

        dueEvent.AbortedEvent += (sender, args) =>
        {
            abortedEventFired = true;
            abortedEventArgs = args;
        };

        // Act
        await dueEvent.StartAsync(CancellationToken.None);
        await Task.Delay(10); 
        await dueEvent.StopAsync(CancellationToken.None);

        // Assert
        abortedEventFired.Should().BeTrue();
        triggeredEventFired.Should().BeFalse();
        abortedEventArgs.Should().NotBeNull();
        abortedEventArgs!.FetcherIdentifier.Should().Be(TestFetcherIdentifier);
        abortedEventArgs.EventIdentifier.Should().Be(dueEvent.EventIdentifier);
        abortedEventArgs.DueAt.Should().Be(dueAt);
    }

    [TestMethod]
    public async Task Events_ShouldOnlyFireOnce_WhenTriggeredEventCompletes()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);
        int triggeredEventCount = 0;
        int abortedEventCount = 0;

        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .ReturnsNextFromSequence(
                _baseDateTimeUtc,          
                dueAt.AddMinutes(1)       
            );

        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        dueEvent.TriggeredEvent += (sender, args) =>
        {
            triggeredEventCount++;
        };

        dueEvent.AbortedEvent += (sender, args) =>
        {
            abortedEventCount++;
        };

        // Act
        await dueEvent.StartAsync(CancellationToken.None);
        await Task.Delay(10); 
        await dueEvent.StopAsync(CancellationToken.None); 

        // Assert
        triggeredEventCount.Should().Be(1);
        abortedEventCount.Should().Be(0);
    }

    [TestMethod]
    public async Task Events_ShouldBeThreadSafe_WhenAccessedConcurrently()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);
        int eventCount = 0;
        object eventLock = new object();
        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .ReturnsNextFromSequence(
                _baseDateTimeUtc,         
                dueAt.AddMinutes(1)    
            );

        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        dueEvent.TriggeredEvent += (sender, args) =>
        {
            lock (eventLock)
            {
                eventCount++;
            }
        };

        Task[] tasks =
        [
            dueEvent.StartAsync(CancellationToken.None),
            Task.Run(async () =>
            {
                await Task.Delay(5);
                await dueEvent.StopAsync(CancellationToken.None);
            })
        ];

        await Task.WhenAll(tasks);

        // Assert
        eventCount.Should().Be(1);
    }

    [TestMethod]
    public async Task ExecuteAsync_ShouldFireTriggeredEvent_WhenDueTimeReachedDuringExecution()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);
        bool triggeredEventFired = false;
        bool abortedEventFired = false;
        DueEventArgs? triggeredEventArgs = null;

        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .ReturnsNextFromSequence(
                _baseDateTimeUtc,       
                _baseDateTimeUtc          
            );

        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        dueEvent.TriggeredEvent += (sender, args) =>
        {
            triggeredEventFired = true;
            triggeredEventArgs = args;
        };

        dueEvent.AbortedEvent += (sender, args) =>
        {
            abortedEventFired = true;
        };

        // Act
        CancellationTokenSource cts = new CancellationTokenSource();
        await dueEvent.StartAsync(cts.Token);
        await Task.Delay(10);
        cts.Cancel(); 
        await dueEvent.StopAsync(CancellationToken.None);
        
        // Assert
        abortedEventFired.Should().BeTrue();
        triggeredEventFired.Should().BeFalse();
    }

    [TestMethod]
    public async Task StopAsync_ShouldCheckCurrentTime_WhenDeterminingEventToFire()
    {
        // Arrange
        DateTime dueAt = _baseDateTimeUtc.AddHours(1);
        
        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .ReturnsNextFromSequence(
                _baseDateTimeUtc,           
                _baseDateTimeUtc,       
                _baseDateTimeUtc.AddMinutes(30), 
                _baseDateTimeUtc.AddMinutes(30)  
            );

        bool abortedEventFired = false;
        
        FetcherSchedulerDueEvent dueEvent = new FetcherSchedulerDueEvent(
            TestFetcherIdentifier, 
            dueAt, 
            _dateTimeFacadeMock, 
            _loggerMock);

        dueEvent.AbortedEvent += (sender, args) =>
        {
            abortedEventFired = true;
        };

        // Act
        await dueEvent.StartAsync(CancellationToken.None);
        await Task.Delay(10); // Minimal delay
        await dueEvent.StopAsync(CancellationToken.None);

        // Assert
        abortedEventFired.Should().BeTrue();
        
        A.CallTo(() => _dateTimeFacadeMock.DateTimeUTCNow())
            .MustHaveHappened(3,Times.OrMore); 
    }
}