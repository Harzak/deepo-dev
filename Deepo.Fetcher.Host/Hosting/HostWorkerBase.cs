using Deepo.Fetcher.Host.Interfaces;
using Deepo.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Host.Hosting;

    public abstract class HostWorkerBase : BackgroundService, IHostWorker
    {
        protected ILogger Logger { get; }

        public string Name { get; set; }
        public Guid Id { get; set; }

        protected HostWorkerBase(ILogger logger)
        {
            Name = "DefaultWorker";
            Logger = logger;
        }

        protected abstract Task StartAllWorkersAsync(CancellationToken cancellationToken);

        protected abstract Task StartManyWorkersAsync(IEnumerable<IWorker> workers, CancellationToken cancellationToken);

        protected abstract Task StartWorkerAsync(IWorker worker, CancellationToken cancellationToken);

        protected abstract Task StopAllWorkersAsync(CancellationToken cancellationToken);

        protected abstract Task StopWorkersAsync(IWorker worker, CancellationToken cancellationToken);
    }
