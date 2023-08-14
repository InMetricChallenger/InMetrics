using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Application.Common.Models;

public class Result
{
    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = new ReadOnlyCollection<string>(errors.ToList());
    }

    public bool Succeeded { get; init; }

    public IReadOnlyCollection<string> Errors { get; init; }   
    
}