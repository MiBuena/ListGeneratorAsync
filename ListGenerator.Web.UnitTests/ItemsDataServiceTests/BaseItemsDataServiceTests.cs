using AutoMapper;
using ListGeneration.Data.Interfaces;
using ListGenerator.Data.Entities;
using ListGenerator.Server.CommonResources;
using ListGenerator.Server.Interfaces;
using ListGenerator.Server.Services;
using ListGenerator.Shared.Dtos;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ListGenerator.Web.UnitTests.ItemsDataServiceTests
{
    public class BaseItemsDataServiceTests : BaseItemsTests
    {
        [SetUp]
        protected virtual void Init()
        {
            ItemsRepositoryMock = new Mock<IItemsRepository>(MockBehavior.Strict);
            UnitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            UnitOfWorkMock.Setup(x => x.ItemsRepository).Returns(ItemsRepositoryMock.Object);

            MapperMock = new Mock<IMapper>(MockBehavior.Strict);
            StringLocalizerMock = new Mock<IStringLocalizer<Errors>>(MockBehavior.Strict);
            ItemsDataService = new ItemsDataService(UnitOfWorkMock.Object, MapperMock.Object, StringLocalizerMock.Object);

            string key = "OverviewItemsError";
            string value = "An error occured while getting items";
            var localizedString = new LocalizedString(key, value);
            StringLocalizerMock.Setup(_ => _[key]).Returns(localizedString);
        }

        protected Mock<IItemsRepository> ItemsRepositoryMock { get; private set; }
        protected Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }
        protected Mock<IMapper> MapperMock { get; private set; }
        protected Mock<IStringLocalizer<Errors>> StringLocalizerMock { get; private set; }
        protected IItemsDataService ItemsDataService { get; private set; }


        protected FilterPatemetersDto BuildParametersDto(int skipItems = 0, int pageSize = 2)
        {
            var filterParameters = new FilterPatemetersDto()
            {
                PageSize = pageSize,
                SkipItems = skipItems
            };

            return filterParameters;
        }

        protected List<ItemOverviewDto> BuildItemOverviewDtosCollection()
        {
            var collection = new List<ItemOverviewDto>();

            var firstItemOverviewDto = new ItemOverviewDto()
            {
                Id = 1,
                Name = "Bread",
                NextReplenishmentDate = new DateTime(2020, 10, 06),
                ReplenishmentPeriod = 1,
                LastReplenishmentDate = new DateTime(2020, 10, 03),
                LastReplenishmentQuantity = 3
            };

            var secondItemOverviewDto = new ItemOverviewDto()
            {
                Id = 2,
                Name = "Cheese",
                NextReplenishmentDate = new DateTime(2020, 10, 08),
                ReplenishmentPeriod = 2,
                LastReplenishmentDate = null,
                LastReplenishmentQuantity = null
            };

            collection.Add(firstItemOverviewDto);
            collection.Add(secondItemOverviewDto);

            return collection;
        }

        protected IQueryable<Item> BuildAdditionalItemsCollection()
        {
            var sixthItemPurchases = BuildSixthItemPurchases();

            var sixthItem = new Item()
            {
                Id = 6,
                Name = "Barley",
                NextReplenishmentDate = new DateTime(2020, 10, 06),
                ReplenishmentPeriod = 2,
                UserId = "ab70793b-cec8-4eba-99f3-cbad0b1649d0",
                Purchases = sixthItemPurchases
            };

            var seventhItem = new Item()
            {
                Id = 7,
                Name = "Bacon",
                NextReplenishmentDate = new DateTime(2020, 10, 2),
                ReplenishmentPeriod = 5,
                UserId = "ab70793b-cec8-4eba-99f3-cbad0b1649d0"
            };

            var collection = new List<Item>() { sixthItem, seventhItem };
            return collection.AsQueryable();
        }

        private ICollection<Purchase> BuildSixthItemPurchases()
        {
            var firstPurchase = new Purchase()
            {
                ReplenishmentDate = new DateTime(2020, 10, 02),
                Quantity = 2,
                ItemId = 6
            };

            var list = new List<Purchase>() { firstPurchase };
            return list;
        }

        protected Item BuildFirstItemWithoutId()
        {
            var firstItem = new Item()
            {
                Name = "Bread",
                NextReplenishmentDate = new DateTime(2020, 10, 06),
                ReplenishmentPeriod = 1,
            };

            return firstItem;
        }

        protected ItemNameDto BuildFirstItemNameDto()
        {
            var firstItemNameDto = new ItemNameDto()
            {
                Name = "Bread",
            };

            return firstItemNameDto;
        }

        protected ItemNameDto BuildSecondItemNameDto()
        {
            var secondItemNameDto = new ItemNameDto()
            {
                Name = "Cheese",
            };

            return secondItemNameDto;
        }

        protected ItemNameDto BuildThirdItemNameDto()
        {
            var secondItemNameDto = new ItemNameDto()
            {
                Name = "Biscuits",
            };

            return secondItemNameDto;
        }



        protected ItemDto BuildFirstItemDtoWithoutId()
        {
            var firstItemDto = new ItemDto()
            {
                Name = "Bread",
                NextReplenishmentDate = new DateTime(2020, 10, 06),
                ReplenishmentPeriod = 1
            };

            return firstItemDto;
        }
    }
}
