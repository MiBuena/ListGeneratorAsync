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

        Task<BaseResponse> AddItem(string userId, ItemDto itemDto);

        Task<BaseResponse> UpdateItemAsync(string userId, ItemDto itemDto);

        Task<BaseResponse> DeleteItem(int id, string userId);

        Task<Response<ItemsOverviewPageDto>> GetItemsOverviewPageModel(string userId, FilterPatemetersDto dto);
    }
}
