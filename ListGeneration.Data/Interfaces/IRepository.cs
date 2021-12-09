using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ListGenerator.Data.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllAsNoTracking();

        Task<int> CountAsync<TSource>([NotNull] IQueryable<TSource> source, CancellationToken cancellationToken = default);

        Task<List<TSource>> ToListAsync<TSource>([NotNull] IQueryable<TSource> source, CancellationToken cancellationToken = default);

        Task<TSource> FirstOrDefaultAsync<TSource>([NotNull] IQueryable<TSource> source, CancellationToken cancellationToken = default);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();

        void SaveChanges();
    }
}
