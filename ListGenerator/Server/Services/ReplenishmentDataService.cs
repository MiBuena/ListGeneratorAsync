using AutoMapper;
using ListGenerator.Shared.Dtos;
using ListGenerator.Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ListGenerator.Data.Entities;
using ListGenerator.Shared.Helpers;
using ListGenerator.Shared.Responses;
using ListGenerator.Server.Builders;
using ListGenerator.Shared.Extensions;
using ListGeneration.Data.Interfaces;

namespace ListGenerator.Server.Services
{
    public class ReplenishmentDataService : IReplenishmentDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IReplenishmentItemBuilder _replenishmentItemBuilder;

        public ReplenishmentDataService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IReplenishmentItemBuilder replenishmentItemBuilder)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _replenishmentItemBuilder = replenishmentItemBuilder;
        }

        public async Task<Response<IEnumerable<ReplenishmentItemDto>>> GetShoppingListAsync(string firstReplenishmentDateAsString, string secondReplenishmentDateAsString, string userId)
        {
            try
            {
                userId.ThrowIfNullOrEmpty();
                firstReplenishmentDateAsString.ThrowIfNullOrEmpty();
                secondReplenishmentDateAsString.ThrowIfNullOrEmpty();

                var firstReplenishmentDate = DateTimeHelper.ToDateFromTransferDateAsString(firstReplenishmentDateAsString);
                var secondReplenishmentDate = DateTimeHelper.ToDateFromTransferDateAsString(secondReplenishmentDateAsString);

                var itemsNeedingReplenishment = await _unitOfWork.ItemsRepository.GetShoppingListItemsAsync(secondReplenishmentDate, userId);
                var replenishmentDtos = _replenishmentItemBuilder.BuildReplenishmentItemsDtos(firstReplenishmentDate, secondReplenishmentDate, itemsNeedingReplenishment);
               
                var response = ResponseBuilder.Success(replenishmentDtos);
                return response;
            }
            catch (Exception ex)
            {
                var response = ResponseBuilder.Failure<IEnumerable<ReplenishmentItemDto>>("An error occured while getting shopping items");
                return response;
            }
        }


        public async Task ReplenishItemsAsync(ReplenishmentDto replenishmentData)
        {
            var allItems = await _unitOfWork.ItemsRepository.GetListAsync();

            foreach (var purchaseItem in replenishmentData.Purchaseitems)
            {
                var purchase = _mapper.Map<PurchaseItemDto, Purchase>(purchaseItem);

                _unitOfWork.PurchasesRepository.Add(purchase);


                var item = allItems.FirstOrDefault(x => x.Id == purchaseItem.ItemId);

                item.NextReplenishmentDate = CalculateNextReplenishmentDateTime(item.ReplenishmentPeriod, purchaseItem.Quantity, item.NextReplenishmentDate, purchaseItem.ReplenishmentDate);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private DateTime CalculateNextReplenishmentDateTime(double itemReplenishmentPeriod, int purchasedQuantity, DateTime previousReplenishmentDate, DateTime replenishmentDate)
        {
            var coveredDays = double.Parse(purchasedQuantity.ToString()) * itemReplenishmentPeriod;

            var baseDateToCalculateNextReplDate = replenishmentDate;

            if (previousReplenishmentDate > replenishmentDate)
            {
                baseDateToCalculateNextReplDate = previousReplenishmentDate;
            }

            var newReplenishmentDate = baseDateToCalculateNextReplDate.AddDays(coveredDays);

            return newReplenishmentDate;
        }
    }
}
