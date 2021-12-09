﻿using ListGenerator.Data.Interfaces;
using ListGeneratorListGenerator.Data.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ListGenerator.Data.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity>
            where TEntity : class
    {
        public EfRepository(ApplicationDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = this.Context.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext Context { get; set; }

        public virtual IQueryable<TEntity> All() => this.DbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();


        public Task<List<TSource>> ToListAsync<TSource>([NotNullAttribute] IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.ToListAsync(cancellationToken);
        }

        public Task<TSource> FirstOrDefaultAsync<TSource>([NotNullAttribute] IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.FirstOrDefaultAsync(cancellationToken);
        }

        public Task<int> CountAsync<TSource>([NotNull] IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.CountAsync(cancellationToken);
        }

        public virtual void Add(TEntity entity) => this.DbSet.Add(entity);       

        public virtual void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => this.DbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

        public void SaveChanges() => this.Context.SaveChanges();
    }
}
