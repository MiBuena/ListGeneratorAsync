using ListGeneration.Data.Interfaces;
using ListGenerator.Data.Entities;
using ListGenerator.Data.Interfaces;
using ListGeneratorListGenerator.Data.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ListGeneration.Data.UnitOfWork
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IAsyncRepository<Item> _itemsRepository;
        private readonly IAsyncRepository<Purchase> _purchasesRepository;

        public UnitOfWork(ApplicationDbContext context, 
            IAsyncRepository<Item> itemsRepository,
            IAsyncRepository<Purchase> purchasesRepository)
        {
            _context = context;
            _itemsRepository = itemsRepository;
            _purchasesRepository = purchasesRepository;
        }


        public IAsyncRepository<Item> ItemsRepository => this._itemsRepository;

        public IAsyncRepository<Purchase> PurchasesRepository => this._purchasesRepository;

        public Task<int> SaveChangesAsync() => this._context.SaveChangesAsync();
    }
}
