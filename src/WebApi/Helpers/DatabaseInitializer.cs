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

        // Verifica se a tabela CashFlow já contém registros
        //if (commandContext.CashFlows.Any())
        //{
        //    return;
        //}

        //if (queryContext.CashFlows.Any())
        //{
        //    return;
        //}

        // Cria alguns registros fictícios
        //const int numberOfCashFlow = 100;
        //var cashFlow = new List<CashFlow>(numberOfCashFlow);

        //for (int i = 1; i <= numberOfCashFlow; i++)
        //{
        //    cashFlow.Add(new CashFlow(i * 2, Domain.Enums.TransactionType.Debit, DateTime.Now));
        //}

        //// Adicione os registros fictícios ao banco de dados e salve as alterações
        //commandContext.CashFlows.AddRange(cashFlow);
        //commandContext.SaveChanges();

        //queryContext.CashFlows.AddRange(cashFlow);
        //queryContext.SaveChanges();
    }
}
