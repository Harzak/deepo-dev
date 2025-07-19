using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Viewer.Interfaces;

/// <summary>
/// Defines the contract for listening to database change operations with event notifications.
/// </summary>
public interface IListener : IDisposable
{
    /// <summary>
    /// Represents a delegate for handling data change events.
    /// </summary>
    public delegate void DataChanged();
    
    /// <summary>
    /// Occurs when a database insert operation is detected.
    /// </summary>
    event EventHandler OnInsert;
    
    /// <summary>
    /// Occurs when a database update operation is detected.
    /// </summary>
    event EventHandler OnUpdate;
    
    /// <summary>
    /// Occurs when a database delete operation is detected.
    /// </summary>
    event EventHandler OnDelete;

    /// <summary>
    /// Starts the listener to begin monitoring database changes.
    /// </summary>
    bool StartListener();

    /// <summary>
    /// Stops the listener and ceases monitoring database changes.
    /// </summary>
    void StopListener();
}