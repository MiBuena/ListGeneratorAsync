using FluentAssertions;
using ListGenerator.Shared.Dtos;
using ListGenerator.Shared.Enums;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public async Task Should_ReturnErrorResponse_When_UserIdIsNullAsync()
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

            //Act
            var response = await ItemsDataService.GetItemsOverviewPageModelAsync(null, filterParameters);

            //Assert
            AssertHelper.AssertAll(
                 () => response.IsSuccess.Should().BeFalse(),
                 () => response.ErrorMessage.Should().Be("An error occured while getting items"),
                 () => response.Data.Should().BeNull()
                 );
        }

        //        //[Test]
        //        //public void Should_ReturnErrorResponse_When_UserIdIsEmpty()
        //        //{
        //        //    //Arrange
        //        //    var filterParameters = new FilterPatemetersDto()
        //        //    {
        //        //        PageSize = 2,
        //        //        SkipItems = 2,
        //        //        OrderByDirection = SortingDirection.Ascending,
        //        //        OrderByColumn = "Name",
        //        //        SearchDate = "02-10-2020",
        //        //        SearchWord = "B"
        //        //    };


        //        //    //Act
        //        //    var response = ItemsDataService.GetItemsOverviewPageModel(string.Empty, filterParameters);


        //        //    //Assert
        //        //    AssertHelper.AssertAll(
        //        //         () => response.IsSuccess.Should().BeFalse(),
        //        //         () => response.ErrorMessage.Should().Be("An error occured while getting items names"),
        //        //         () => response.Data.Should().BeNull()
        //        //         );
        //        //}

        //        //[Test]
        //        //public void Should_ReturnErrorResponse_When_FilterDtoIsNull()
        //        //{
        //        //    //Arrange
        //        //    var filterParameters = new FilterPatemetersDto()
        //        //    {
        //        //        PageSize = 2,
        //        //        SkipItems = 2,
        //        //        OrderByDirection = SortingDirection.Ascending,
        //        //        OrderByColumn = "Name",
        //        //        SearchDate = "02-10-2020",
        //        //        SearchWord = "B"
        //        //    };


        //        //    //Act
        //        //    var response = ItemsDataService.GetItemsOverviewPageModel("ab70793b-cec8-4eba-99f3-cbad0b1649d0", null);


        //        //    //Assert
        //        //    AssertHelper.AssertAll(
        //        //         () => response.IsSuccess.Should().BeFalse(),
        //        //         () => response.ErrorMessage.Should().Be("An error occured while getting items names"),
        //        //         () => response.Data.Should().BeNull()
        //        //         );
        //        //}


        //        [Test]
        //        public void Should_ReturnSuccessResponse_When_UserDoesNotHaveItems()
        //        {
        //            //Arrange
        //            var allItems = new List<Item>().AsQueryable();
        //            ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //            var filterParameters = new FilterPatemetersDto()
        //            {
        //                PageSize = 2,
        //                SkipItems = 0,
        //                OrderByDirection = SortingDirection.Ascending,
        //                OrderByColumn = "Name",
        //                SearchDate = "02-10-2020",
        //                SearchWord = "B"
        //            };


        //            //Act
        //            var response = ItemsDataService.GetItemsOverviewPageModel("ab70793b-cec8-4eba-99f3-cbad0b1649d0", filterParameters);


        //            //Assert
        //            AssertHelper.AssertAll(
        //                 () => response.IsSuccess.Should().BeTrue(),
        //                 () => response.ErrorMessage.Should().BeNull(),
        //                 () => response.Data.TotalItemsCount.Should().Be(0),
        //                 () => response.Data.OverviewItems.Count().Should().Be(0)
        //                 );
        //        }
    }
    }
