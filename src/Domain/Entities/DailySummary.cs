using Domain.Common;
using MediatR;
using System.Collections.ObjectModel;

namespace Domain.Entities;
public class DailySummary : BaseAuditableEntity
{   

    public DateTime Date { get; private set; }

    public decimal TotalCredit { get; private set; }

    public decimal TotalDebit { get; private set; }

    public decimal NetAmount => TotalCredit - TotalDebit;

    public DailySummary()
    {
        
    }

    public DailySummary(int id, DateTime date)
    {
        Id = id;
        Date = date;
    }

    public void AddCredit(decimal amount)
    {        
        TotalCredit += amount;
    }

    public void AddDebit(decimal amount)
    {        
        TotalDebit += amount;
    }

    public DailySummary(DateTime date)
    {
        Date = date;
        TotalCredit = 0;
        TotalDebit = 0;
    }
}
