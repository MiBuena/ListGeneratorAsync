using AutoMapper;
using ListGenerator.Shared.Dtos;
using ListGenerator.Server.Interfaces;
using System;
using System.Collections.Generic;
using ListGenerator.Data.Entities;
using ListGenerator.Shared.Responses;
using ListGenerator.Server.Builders;
using ListGenerator.Shared.Extensions;
using ListGenerator.Shared.CustomExceptions;
using ListGenerator.Server.CommonResources;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using ListGeneration.Data.Interfaces;

namespace ListGenerator.Server.Services
{
    public class ItemsDataService : IItemsDataService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<Errors> _localizer;


        public ItemsDataService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IStringLocalizer<Errors> localizer = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

                var pageDto = await _unitOfWork.ItemsRepository.GetItemsOverviewPageDtosAsync(userId, dto);
              
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

        public async Task<BaseResponse> UpdateItemAsync(string userId, ItemDto itemDto)
        {
            try
            {
                userId.ThrowIfNullOrEmpty();
                itemDto.ThrowIfNull();

                var itemToUpdate = await _unitOfWork.ItemsRepository
                    .FirstOrDefaultAsync(x => x.Id == itemDto.Id && x.UserId == userId);

                itemToUpdate.ThrowIfNullWithShowMessage($"Current user does not have item with id {itemDto.Id}");

                itemToUpdate.Name = itemDto.Name;
                itemToUpdate.ReplenishmentPeriod = itemDto.ReplenishmentPeriod;
                itemToUpdate.NextReplenishmentDate = itemDto.NextReplenishmentDate;
                itemToUpdate.UserId = userId;

                _unitOfWork.ItemsRepository.Update(itemToUpdate);
                await _unitOfWork.SaveChangesAsync();


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

        public async Task<BaseResponse> DeleteItemAsync(int id, string userId)
        {
            try
            {
                userId.ThrowIfNullOrEmpty();

                var itemToDelete = await _unitOfWork.ItemsRepository
                    .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

                itemToDelete.ThrowIfNullWithShowMessage($"Current user does not have item with id {id}");

                _unitOfWork.ItemsRepository.Delete(itemToDelete);
                await _unitOfWork.SaveChangesAsync();

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
