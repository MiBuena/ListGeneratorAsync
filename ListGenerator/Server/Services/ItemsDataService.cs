using AutoMapper;
using ListGenerator.Shared.Dtos;
using ListGenerator.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ListGenerator.Data.Interfaces;
using ListGenerator.Data.Entities;
using ListGenerator.Server.Extensions;
using ListGenerator.Shared.Enums;
using ListGenerator.Shared.Helpers;
using ListGenerator.Shared.Responses;
using ListGenerator.Client.Builders;
using ListGenerator.Server.Builders;
using ListGenerator.Shared.Extensions;
using ListGenerator.Shared.CustomExceptions;
using ListGenerator.Server.CommonResources;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using ListGenerator.Shared.Interfaces;
using ListGeneration.Data.Interfaces;

namespace ListGenerator.Server.Services
{
    public class ItemsDataService : IItemsDataService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Item> _itemsRepository;
        private readonly IStringLocalizer<Errors> _localizer;


        public ItemsDataService(IUnitOfWork unitOfWork,
            IRepository<Item> itemsRepository,
            IMapper mapper,
            IStringLocalizer<Errors> localizer = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _itemsRepository = itemsRepository;
            _localizer = localizer;
        }

        public async Task<Response<IEnumerable<ItemNameDto>>> GetItemsNamesAsync(string searchWord, string userId)
        {
            try
            {
                searchWord.ThrowIfNull();
                userId.ThrowIfNullOrEmpty();

                var names = await _unitOfWork.ItemsRepository.GetItemsNamesDtosAsync(searchWord, userId);

                var response = ResponseBuilder.Success(names);
                return response;
            }
            catch (Exception ex)
            {
                var response = ResponseBuilder.Failure<IEnumerable<ItemNameDto>>("An error occured while getting items names.");
                return response;
            }
        }

        public async Task<Response<ItemsOverviewPageDto>> GetItemsOverviewPageModel(string userId, FilterPatemetersDto dto)
        {
            try
            {
                userId.ThrowIfNullOrEmpty();
                dto.ThrowIfNull();

                var query = GetOverviewItemsQuery(userId, dto);

                var pagedQuery = query
                    .Skip(dto.SkipItems.Value)
                    .Take(dto.PageSize.Value);

                var dtos = await _itemsRepository.ToListAsync(pagedQuery);
                    
                var itemsCount = await _itemsRepository.CountAsync(query);

                var pageDto = new ItemsOverviewPageDto()
                {
                    OverviewItems = dtos,
                    TotalItemsCount = itemsCount
                };

                var response = ResponseBuilder.Success(pageDto);
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = _localizer["OverviewItemsError"];
                var response = ResponseBuilder.Failure<ItemsOverviewPageDto>(errorMessage);
                return response;
            }
        }

        private IQueryable<ItemOverviewDto> GetOverviewItemsQuery(string userId, FilterPatemetersDto dto)
        {
            var query = GetBaseQuery(userId);

            query = FilterBySearchWord(dto.SearchWord, query);

            query = FilterBySearchDate(dto.SearchDate, query);

            query = Sort(dto.OrderByColumn, dto.OrderByDirection, query);

            return query;
        }

        private IQueryable<ItemOverviewDto> FilterBySearchDate(string searchDate, IQueryable<ItemOverviewDto> query)
        {
            if (searchDate != null)
            {
                var parsedDate = DateTimeHelper.ToDateFromTransferDateAsString(searchDate);

                query = query
                    .Where(x => x.LastReplenishmentDate == parsedDate
                || x.NextReplenishmentDate == parsedDate);
            }

            return query;
        }

        private IQueryable<ItemOverviewDto> FilterBySearchWord(string searchWord, IQueryable<ItemOverviewDto> query)
        {
            if (searchWord != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchWord.ToLower()));
            }

            return query;
        }

        private IQueryable<ItemOverviewDto> Sort(string orderByColumn, SortingDirection? orderByDirection, IQueryable<ItemOverviewDto> query)
        {
            if (orderByColumn != null && orderByDirection != null)
            {
                if (orderByDirection == SortingDirection.Ascending)
                {
                    query = query.OrderByProperty(orderByColumn);
                }
                else
                {
                    query = query.OrderByPropertyDescending(orderByColumn);
                }
            }

            return query;
        }

        private IQueryable<ItemOverviewDto> GetBaseQuery(string userId)
        {
            var query = _itemsRepository.All()
                .Where(x => x.UserId == userId)
                .Select(x => new ItemOverviewDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ReplenishmentPeriod = x.ReplenishmentPeriod,
                    NextReplenishmentDate = x.NextReplenishmentDate,
                    LastReplenishmentDate = x.Purchases
                                     .OrderByDescending(y => y.ReplenishmentDate)
                                     .Select(m => (DateTime?) m.ReplenishmentDate)
                                     .FirstOrDefault(),
                    LastReplenishmentQuantity = x.Purchases
                                     .OrderByDescending(y => y.ReplenishmentDate)
                                     .Select(m => (int?) m.Quantity)
                                     .FirstOrDefault(),
                });

            return query;
        }

        public async Task<Response<ItemDto>> GetItemAsync(int itemId, string userId)
        {
            try
            {
                var dto = await _unitOfWork.ItemsRepository.GetItemDtoAsync(itemId, userId);

                dto.ThrowIfNullWithShowMessage($"Current user does not have item with id {itemId}");

                var response = ResponseBuilder.Success(dto);
                return response;
            }
            catch (ShowErrorMessageException ex)
            {
                var response = ResponseBuilder.Failure<ItemDto>(ex.Message);
                return response;
            }
            catch (Exception ex)
            {
                var response = ResponseBuilder.Failure<ItemDto>("An error occured while getting item");
                return response;
            }
        }

        public async Task<BaseResponse> AddItem(string userId, ItemDto itemDto)
        {
            try
            {
                userId.ThrowIfNullOrEmpty();
                itemDto.ThrowIfNull();

                var itemEntity = _mapper.Map<ItemDto, Item>(itemDto);
                itemEntity.UserId = userId;

                _unitOfWork.ItemsRepository.Add(itemEntity);
                await _unitOfWork.SaveChangesAsync();

                var response = ResponseBuilder.Success();
                return response;
            }
            catch (Exception ex)
            {
                var response = ResponseBuilder.Failure("An error occurred while creating item");
                return response;
            }
        }

        public BaseResponse UpdateItem(string userId, ItemDto itemDto)
        {
            try
            {
                userId.ThrowIfNullOrEmpty();
                itemDto.ThrowIfNull();

                var itemToUpdate = _itemsRepository.All()
                    .FirstOrDefault(x => x.Id == itemDto.Id && x.UserId == userId);

                itemToUpdate.ThrowIfNullWithShowMessage($"Current user does not have item with id {itemDto.Id}");

                itemToUpdate.Name = itemDto.Name;
                itemToUpdate.ReplenishmentPeriod = itemDto.ReplenishmentPeriod;
                itemToUpdate.NextReplenishmentDate = itemDto.NextReplenishmentDate;
                itemToUpdate.UserId = userId;

                _itemsRepository.Update(itemToUpdate);
                _itemsRepository.SaveChanges();


                var response = ResponseBuilder.Success();
                return response;
            }
            catch (ShowErrorMessageException ex)
            {
                var response = ResponseBuilder.Failure<ItemDto>(ex.Message);
                return response;
            }
            catch (Exception ex)
            {
                var response = ResponseBuilder.Failure("An error occurred while updating item");
                return response;
            }
        }

        public BaseResponse DeleteItem(int id, string userId)
        {
            try
            {
                userId.ThrowIfNullOrEmpty();

                var itemToDelete = _itemsRepository.All()
                    .FirstOrDefault(x => x.Id == id && x.UserId == userId);

                itemToDelete.ThrowIfNullWithShowMessage($"Current user does not have item with id {id}");

                _itemsRepository.Delete(itemToDelete);
                _itemsRepository.SaveChanges();

                var response = ResponseBuilder.Success();
                return response;
            }
            catch (ShowErrorMessageException ex)
            {
                var response = ResponseBuilder.Failure<ItemDto>(ex.Message);
                return response;
            }
            catch (Exception ex)
            {
                var response = ResponseBuilder.Failure("An error occurred while deleting item");
                return response;
            }
        }
    }
}
