using Deepo.Fetcher.Viewer.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Data.SqlClient;
using Deepo.Fetcher.Viewer.Exceptions;

namespace Deepo.Fetcher.Viewer.Features.Listener;

/// <summary>
/// Provides SQL Server database change monitoring capabilities using SqlDependency notifications.
/// </summary>
public class SQLListener : IListener, IDisposable
{
    /// <summary>
    /// Occurs when a database insert operation is detected.
    /// </summary>
    public event EventHandler? OnInsert;
    
    /// <summary>
    /// Occurs when a database update operation is detected.
    /// </summary>
    public event EventHandler? OnUpdate;
    
    /// <summary>
    /// Occurs when a database delete operation is detected.
    /// </summary>
    public event EventHandler? OnDelete;

    private readonly ILogger _logger;

    private readonly string _connectionString;
    private readonly string _subsriptionRequest;

    private SQLDependency? _sqlDependency;

    private bool disposedValue;

    public SQLListener(string connectionString, string subsriptionRequest, ILogger logger)
    {
        _logger = logger;
        _connectionString = connectionString;
        _subsriptionRequest = subsriptionRequest;
    }

    /// <summary>
    /// Starts the SQL dependency listener for database change notifications.
    /// </summary>
    public bool StartListener()
    {
        SqlDependency.Start(_connectionString);
        return Subscribe();
    }

    /// <summary>
    /// Stops the SQL dependency listener and cleans up resources.
    /// </summary>
    public void StopListener()
    {
        SqlDependency.Stop(_connectionString);
    }

    private bool Subscribe()
    {
        _sqlDependency =  new SQLDependency(_connectionString, _subsriptionRequest, _logger);

        _sqlDependency.OnChange += Depency_OnChange;
        return _sqlDependency.Subscribe();

    }

    private void Notify(SqlNotificationEventArgs e)
    {
        switch (e.Info)
        {
            case SqlNotificationInfo.Insert:
                OnInsert?.Invoke(this, e);
                break;
            case SqlNotificationInfo.Update:
                OnUpdate?.Invoke(this, e);
                break;
            case SqlNotificationInfo.Delete:
                OnDelete?.Invoke(this, e);
                break;
            default:
                break;
        }
    }

    private void Depency_OnChange(object sender, SqlNotificationEventArgs e)
    {
        switch (e.Type)
        {
            case SqlNotificationType.Change:
                Notify(e);
                break;
            case SqlNotificationType.Subscribe:
                if (e.Info ==  SqlNotificationInfo.Invalid)
                {
                    throw new InvalideSQLSubscriptionException($"Invalid Subscription. Error detected on: {e.Source}");
                }
                break;
            default:
                break;
        }
        Subscribe();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _sqlDependency?.Dispose();
            }

            disposedValue=true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
