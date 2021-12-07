using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ListGenerator.Shared.Interfaces
{
    public interface IAsyncConverter
    {
        Task<List<TSource>> ConvertToListAsync<TSource>([NotNullAttribute] IQueryable<TSource> source, CancellationToken cancellationToken = default);
    }
}
