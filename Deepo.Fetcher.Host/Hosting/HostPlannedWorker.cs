using Deepo.Framework.EventArguments;
using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Host.Hosting;

    public abstract class HostPlannedWorker : HostWorker
    {
        protected IScheduler Scheduler { get; }

        protected HostPlannedWorker(string name, Guid id, IScheduler scheduler, ILogger logger) : base(name, id, logger)
        {
            Scheduler = scheduler;
            Scheduler.ReadyToStart += OnWorkerReadyToStartAsync;
        }

        private async void OnWorkerReadyToStartAsync(object? sender, WorkerEventArgs? e)
        {
            if (e != null && e.Worker != null)
            {
                await StartWorkerAsync(e.Worker, new CancellationToken()).ConfigureAwait(false);
            }
        }

        protected async override Task StartWorkerAsync(IWorker worker, CancellationToken cancellationToken)
        {
            if (worker is null) return;

            await base.StartWorkerAsync(worker, cancellationToken).ConfigureAwait(false);
        }

        protected abstract override bool CanStart();

        protected abstract override Task ExecuteAsync(CancellationToken stoppingToken);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Scheduler.ReadyToStart -= OnWorkerReadyToStartAsync;
        }
    }
