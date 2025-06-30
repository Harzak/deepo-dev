using Deepo.Fetcher.Library.Interfaces;
using Framework.Common.Utils.Result;

namespace Deepo.Fetcher.Library.Fetcher.Fetch;

internal abstract class FetchBase : IFetch, IDisposable
{
    private Action _onSuccesBatch;
    private Action _onErrorBatch;
    private Func<bool>? _canContinueBatch;
    private bool disposedValue;
    private readonly bool _continueOnError;

    protected bool FetchInExecution { get; private set; }

    protected IResult? CurrentResult { get; private set; }

    private DateTime _minReleaseDate;
    public DateTime MinReleaseDate
    {
        get => _minReleaseDate;
        set
        {
            if (_minReleaseDate != value && !FetchInExecution)
            {
                _minReleaseDate = value;
            }
        }
    }

    public int Count { get; private set; }
    public bool Success
    {
        get => CurrentResult != null && CurrentResult.IsSuccess;
    }

    public FetchBase()
    {
        _onSuccesBatch = null!;
        _onErrorBatch = null!;
        _minReleaseDate = DateTime.UtcNow.AddDays(-30);
    }

    public FetchBase(bool continueOnError) : this()
    {
        _continueOnError = continueOnError;
    }

    public abstract Task StartAsync(CancellationToken cancellationToken);

    protected virtual bool CanStart()
    {
        return CurrentResult is null;
    }

    protected async Task<IFetch> StartWithAsync(Func<Task<IResult>> action)
    {
        if (CanStart())
        {
            FetchInExecution = true;
            CurrentResult = await action().ConfigureAwait(false);
        }
        return this;
    }

    protected async Task<IFetch> ContinueWithAsync(Func<Task<IResult>> action)
    {
        if (CanContinue())
        {
            CurrentResult = await action().ConfigureAwait(false);
            if (!Success)
            {
                _onErrorBatch?.Invoke();
            }
            Count++;
            _canContinueBatch = null;
        }
        return this;
    }

    protected async Task<IFetch> EndWith(Func<Task<IResult>> action)
    {
        await ContinueWithAsync(action).ConfigureAwait(false);
        if (Success)
        {
            _onSuccesBatch?.Invoke();
        }
        FetchInExecution = false;
        return this;
    }
    protected FetchBase OnError(Action action)
    {
        _onErrorBatch = action;
        return this;
    }

    protected FetchBase OnSucces(Action action)
    {
        _onSuccesBatch = action;
        return this;
    }

    protected FetchBase CanContinue(Func<bool> action)
    {
        _canContinueBatch = action;
        return this;
    }

    private bool CanContinue()
    {
        bool currentOpIsInSucces = CurrentResult != null && CurrentResult.IsSuccess;

        if (currentOpIsInSucces)
        {
            return _canContinueBatch is null || _canContinueBatch();
        }

        return _continueOnError;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            _onSuccesBatch = null!;
            _onErrorBatch = null!;
            _canContinueBatch = null!;
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
