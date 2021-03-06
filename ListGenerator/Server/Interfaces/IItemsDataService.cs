using ListGenerator.Shared.Dtos;
using ListGenerator.Shared.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ListGenerator.Server.Interfaces
{
    public interface IItemsDataService
    {
        Task<Response<IEnumerable<ItemNameDto>>> GetItemsNamesAsync(string searchWord, string userId);

        Task<Response<ItemDto>> GetItemAsync(int itemId, string userId);

        Task<BaseResponse> AddItemAsync(string userId, ItemDto itemDto);

        Task<BaseResponse> UpdateItemAsync(string userId, ItemDto itemDto);

        Task<BaseResponse> DeleteItemAsync(int id, string userId);

        Task<Response<ItemsOverviewPageDto>> GetItemsOverviewPageModelAsync(string userId, FilterPatemetersDto dto);
    }
}
