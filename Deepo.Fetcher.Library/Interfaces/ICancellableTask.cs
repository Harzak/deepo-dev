namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Interface for tasks that support cancellation operations
/// </summary>
public interface ICancellableTask
{
    /// <summary>
    /// Cancels the task execution asynchronously with an optional reason.
    /// </summary>
    /// <param name="reason">Optional reason for the cancellation.</param>
    Task Cancel(CancellationToken cancellationToken, string reason = "");
}