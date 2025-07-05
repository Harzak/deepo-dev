using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Deepo.Fetcher.Viewer.LogMessages;

namespace Deepo.Fetcher.Viewer.Features.Listener;

internal class SQLDependency : IDisposable
{
    public int NumberRetryOpenConnection { get; set; }

    internal event OnChangeEventHandler? OnChange;

    private readonly ILogger _logger;

    private int _numberOpenConnectionRetry;
    private readonly string _connectionString;
    private readonly string _subsriptionRequest;

    private SqlDependency? _dependency;
    private SqlConnection? _connection;
    private SqlCommand? _command;

    internal SQLDependency(string connectionString, string subsriptionRequest, ILogger logger)
    {
        NumberRetryOpenConnection = 3;

        _logger = logger;
        _connectionString = connectionString;
        _subsriptionRequest = subsriptionRequest;
    }

    internal bool Subscribe()
    {
        return OpenConnection() && CreateCommand() && CreateDependency() && ExecuteCommand();
    }

    internal void Unsubscribe()
    {
        _command?.Cancel();
        _command?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }

    private bool ExecuteCommand()
    {
        try
        {
            _command?.ExecuteReader();

        }
        catch (SqlException ex)
        {
            DataLogs.UnableExecuteCommand(_logger, _subsriptionRequest, ex);
            return false;
        }
        catch (Exception)
        {
            throw;
        }

        return true;
    }

    protected virtual bool CreateCommand()
    {
        _command = new SqlCommand(_subsriptionRequest, _connection);
        return true;
    }

    protected virtual bool CreateDependency()
    {
        if (_command is null) return false;

        try
        {
            _dependency = new SqlDependency(_command);
            _dependency.OnChange += OnChange;
            return true;
        }
        catch (InvalidOperationException ex)
        {
            DataLogs.UnableCreateDependency(_logger, _subsriptionRequest, ex);
            return false;
        }
        catch (Exception ex)
        {
            DataLogs.UnableCreateDependency(_logger, _subsriptionRequest, ex);
            throw;
        }

    }

    private bool OpenConnection()
    {
        _connection = new SqlConnection(_connectionString);

        try
        {
            _connection.Open();
        }
        catch (InvalidOperationException ex)
        {
            _connection.Close();
            DataLogs.UnableOpenConnection(_logger, _connectionString, ex);
            RetryOpenConnection(_numberOpenConnectionRetry++);
            return false;
        }
        catch (SqlException ex)
        {
            DataLogs.UnableOpenConnection(_logger, _connectionString, ex);
            return false;
        }
        catch (Exception ex)
        {
            DataLogs.UnableOpenConnection(_logger, _connectionString, ex);
            throw;
        }
        return true;
    }

    private void RetryOpenConnection(int retry)
    {
        if (retry <= NumberRetryOpenConnection)
        {
            OpenConnection();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Unsubscribe();
        }
    }
}
