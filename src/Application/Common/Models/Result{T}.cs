using Newtonsoft.Json;

namespace Application.Common.Models;

public class Result<T> : Result
{
    [JsonConstructor]
    internal Result(bool succeeded, T data, IEnumerable<string> errors)
        : base(succeeded, errors)
    {
        Data = data;
    }

    public T Data { get; init; }
}