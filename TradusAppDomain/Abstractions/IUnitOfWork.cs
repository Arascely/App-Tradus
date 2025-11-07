using System.Threading;
using System.Threading.Tasks;

namespace TradusApp.Domain.Abstractions;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
