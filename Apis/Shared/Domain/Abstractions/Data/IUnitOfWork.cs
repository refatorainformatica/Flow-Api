using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Shared.Domain.Abstractions.Data
{
    public interface IUnitOfWork<TContext>
        where TContext : DbContext
    {
        Task ExecuteTransactionAsync(Func<Task> action);
    }
}
