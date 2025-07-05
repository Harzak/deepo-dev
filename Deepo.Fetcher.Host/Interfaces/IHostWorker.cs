namespace Deepo.Fetcher.Host.Interfaces;

public interface IHostWorker : IHostedService, IDisposable
{
    string Name { get; set; }
    Guid Id { get; set; }
}