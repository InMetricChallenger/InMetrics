namespace Application.Common.Interfaces;

public interface IAsyncRetryPolicy
{
    Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> action, CancellationToken cancellationToken);
}
