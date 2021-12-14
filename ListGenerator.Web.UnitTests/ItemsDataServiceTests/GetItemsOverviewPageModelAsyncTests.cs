using FluentAssertions;
using ListGenerator.Data.Entities;
using ListGenerator.Shared.Dtos;
using ListGenerator.Shared.Enums;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ListGenerator.Web.UnitTests.ItemsDataServiceTests
{
    [TestFixture]
    public class GetItemsOverviewPageModelAsyncTests : BaseItemsDataServiceTests
    {
        [SetUp]
        protected override void Init()
        {
            base.Init();
        }

        [Test]
        public async Task Should_ReturnResponseWithFirstPageUserItems_When_FirstPage()
        {
            //Arrange
            var overviewPageDto = new ItemsOverviewPageDto()
            {
                OverviewItems = BuildItemOverviewDtosCollection(),
                TotalItemsCount = 3
            };
            var filterParameters = BuildParametersDto();
            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", filterParameters))
                .ReturnsAsync(overviewPageDto);

            //Act
            var response = await ItemsDataService.GetItemsOverviewPageModelAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", filterParameters);


            //Assert
            AssertHelper.AssertAll(
                 () => response.Data.OverviewItems.Count().Should().Be(2),
                 () => response.Data.OverviewItems.First().Id.Should().Be(1),
                 () => response.Data.OverviewItems.First().Name.Should().Be("Bread"),
                 () => response.Data.OverviewItems.First().NextReplenishmentDate.Should().BeSameDateAs(new DateTime(2020, 10, 06)),
                 () => response.Data.OverviewItems.First().ReplenishmentPeriod.Should().Be(1),
                 () => response.Data.OverviewItems.First().LastReplenishmentDate.Should().BeSameDateAs(new DateTime(2020, 10, 03)),
                 () => response.Data.OverviewItems.First().LastReplenishmentQuantity.Should().Be(3),

                 () => response.Data.OverviewItems.Skip(1).First().Id.Should().Be(2),
                 () => response.Data.OverviewItems.Skip(1).First().Name.Should().Be("Cheese"),
                 () => response.Data.OverviewItems.Skip(1).First().NextReplenishmentDate.Should().BeSameDateAs(new DateTime(2020, 10, 08)),
                 () => response.Data.OverviewItems.Skip(1).First().ReplenishmentPeriod.Should().Be(2),
                 () => response.Data.OverviewItems.Skip(1).First().LastReplenishmentDate.Should().BeNull(),
                 () => response.Data.OverviewItems.Skip(1).First().LastReplenishmentQuantity.Should().BeNull()
                 );
        }

        [Test]
        public async Task Should_ReturnCorrectTotalItemsCount_When_FirstPage()
        {
            //Arrange
            var overviewPageDto = new ItemsOverviewPageDto()
            {
                OverviewItems = BuildItemOverviewDtosCollection(),
                TotalItemsCount = 3
            };
            var filterParameters = BuildParametersDto();
            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", filterParameters))
                .ReturnsAsync(overviewPageDto);

            //Act
            var response = await ItemsDataService.GetItemsOverviewPageModelAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", filterParameters);

            //Assert
            response.Data.TotalItemsCount.Should().Be(3);
        }


        [Test]
        public async Task Should_ReturnSuccessResponse_When_FirstPage()
        {
            //Arrange
            var overviewPageDto = new ItemsOverviewPageDto()
            {
                OverviewItems = BuildItemOverviewDtosCollection(),
                TotalItemsCount = 3
            };
            var filterParameters = BuildParametersDto();
            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", filterParameters))
                .ReturnsAsync(overviewPageDto);

            //Act
            var response = await ItemsDataService.GetItemsOverviewPageModelAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", filterParameters);


            //Assert
            AssertHelper.AssertAll(
                 () => response.IsSuccess.Should().BeTrue(),
                 () => response.ErrorMessage.Should().BeNull()
                 );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_UserIdIsNull()
        {
            //Arrange
            var filterParameters = new FilterPatemetersDto()
            {
                PageSize = 2,
                SkipItems = 2,
                OrderByDirection = SortingDirection.Ascending,
                OrderByColumn = "Name",
                SearchDate = "02-10-2020",
                SearchWord = "B"
            };

            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync(It.IsAny<string>(), filterParameters))
                .ReturnsAsync(new ItemsOverviewPageDto());


            //Act
            var response = await ItemsDataService.GetItemsOverviewPageModelAsync(null, filterParameters);

            //Assert
            AssertHelper.AssertAll(
                 () => response.IsSuccess.Should().BeFalse(),
                 () => response.ErrorMessage.Should().Be("An error occured while getting items"),
                 () => response.Data.Should().BeNull()
                 );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_UserIdIsEmpty()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync(It.IsAny<string>(), It.IsAny<FilterPatemetersDto>()))
                .ReturnsAsync(new ItemsOverviewPageDto());

            var filterParameters = new FilterPatemetersDto()
            {
                PageSize = 2,
                SkipItems = 2,
                OrderByDirection = SortingDirection.Ascending,
                OrderByColumn = "Name",
                SearchDate = "02-10-2020",
                SearchWord = "B"
            };

            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync(It.IsAny<string>(), filterParameters))
                .ReturnsAsync(new ItemsOverviewPageDto());


            //Act
            var response = await ItemsDataService.GetItemsOverviewPageModelAsync(string.Empty, filterParameters);


            //Assert
            AssertHelper.AssertAll(
                 () => response.IsSuccess.Should().BeFalse(),
                 () => response.ErrorMessage.Should().Be("An error occured while getting items"),
                 () => response.Data.Should().BeNull()
                 );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_FilterDtoIsNull()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync(It.IsAny<string>(), It.IsAny<FilterPatemetersDto>()))
                .ReturnsAsync(new ItemsOverviewPageDto());

            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync(It.IsAny<string>(), It.IsAny<FilterPatemetersDto>()))
                .ReturnsAsync(new ItemsOverviewPageDto());

            //Act
            var response = await ItemsDataService.GetItemsOverviewPageModelAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", null);

            //Assert
            AssertHelper.AssertAll(
                 () => response.IsSuccess.Should().BeFalse(),
                 () => response.ErrorMessage.Should().Be("An error occured while getting items"),
                 () => response.Data.Should().BeNull()
                 );
        }


        [Test]
        public async Task Should_ReturnSuccessResponse_When_UserDoesNotHaveItems()
        {
            //Arrange
            var filterParameters = new FilterPatemetersDto()
            {
                PageSize = 2,
                SkipItems = 2,
                OrderByDirection = SortingDirection.Ascending,
                OrderByColumn = "Name",
                SearchDate = "02-10-2020",
                SearchWord = "B"
            };

            ItemsRepositoryMock
                .Setup(x => x.GetItemsOverviewPageDtosAsync(It.IsAny<string>(), filterParameters))
                .ReturnsAsync(new ItemsOverviewPageDto());

            //Act
            var response = await ItemsDataService.GetItemsOverviewPageModelAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", filterParameters);


            //Assert
            AssertHelper.AssertAll(
                 () => response.IsSuccess.Should().BeTrue(),
                 () => response.ErrorMessage.Should().BeNull(),
                 () => response.Data.TotalItemsCount.Should().Be(0),
                 () => response.Data.OverviewItems.Count().Should().Be(0)
                 );
        }
    }
}
