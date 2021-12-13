using ListGenerator.Data.Entities;
using System.Threading.Tasks;

namespace ListGeneration.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IItemsRepository ItemsRepository { get; }

        IAsyncRepository<Purchase> PurchasesRepository { get; }

        Task<int> SaveChangesAsync();
    }
}
