using ListGenerator.Shared.Dtos;
using ListGenerator.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListGenerator.Server.Interfaces
{
    public interface IReplenishmentDataService
    {
        Task<Response<IEnumerable<ReplenishmentItemDto>>> GetShoppingListAsync(string firstReplenishmentDateAsString, string secondReplenishmentDateAsString, string userId);

        Task ReplenishItemsAsync(ReplenishmentDto dto);
    }
}
