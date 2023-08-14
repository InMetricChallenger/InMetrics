using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class CashFlowRepository : BaseRepository<CashFlow>, ICashFlowRepository
{
    public CashFlowRepository(AppCommandDbContext commandContext, AppQueryDbContext queryContext) : base(commandContext, queryContext)
    {
          
    }    
}
