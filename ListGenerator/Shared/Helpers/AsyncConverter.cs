using ListGenerator.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ListGenerator.Shared.Helpers
{
    public class AsyncConverter : IAsyncConverter
    {
        public Task<List<TSource>> ConvertToListAsync<TSource>(IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return source.ToListAsync(cancellationToken);
        }
    }
}
