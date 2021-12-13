using ListGeneration.Data.Interfaces;
using ListGenerator.Data.Entities;
using ListGeneratorListGenerator.Data.DB;
using System.Threading.Tasks;

namespace ListGeneration.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemsRepository _itemsRepository;
        private readonly IAsyncRepository<Purchase> _purchasesRepository;

        public UnitOfWork(ApplicationDbContext context, 
            IItemsRepository itemsRepository,
            IAsyncRepository<Purchase> purchasesRepository)
        {
            _context = context;
            _itemsRepository = itemsRepository;
            _purchasesRepository = purchasesRepository;
        }


        public IItemsRepository ItemsRepository => this._itemsRepository;

        public IAsyncRepository<Purchase> PurchasesRepository => this._purchasesRepository;

        public Task<int> SaveChangesAsync() => this._context.SaveChangesAsync();
    }
}
