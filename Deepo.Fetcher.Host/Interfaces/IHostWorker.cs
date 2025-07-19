namespace Deepo.Fetcher.Viewer.Interfaces;

public interface IHostWorker : IHostedService, IDisposable
{
    string Name { get; set; }
    Guid Id { get; set; }
}