using Domain.Entities;
using Infrastructure.Persistance;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Helpers;

/// <summary>
/// Essa classe é utilizada apenas para efeitos de demonstração para popular registros fakes no banco de dados.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DatabaseInitializer
{
    public static void Initialize(AppCommandDbContext commandContext, AppQueryDbContext queryContext)
    {
        if (queryContext is null)
        {
            throw new ArgumentNullException(nameof(queryContext));
        }

        if (commandContext is null)
        {
            throw new ArgumentNullException(nameof(commandContext));
        }
    }
}
