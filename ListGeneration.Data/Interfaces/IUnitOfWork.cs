using ListGenerator.Data.Entities;
using ListGenerator.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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
