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
