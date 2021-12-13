using AutoMapper;
using FluentAssertions;
using ListGeneration.Data.Interfaces;
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
using System.Text;
using System.Threading.Tasks;

namespace ListGenerator.Web.UnitTests.ItemsDataServiceTests
{
    [TestFixture]
    public class GetItemsNamesAsyncTests : BaseItemsDataServiceTests
    {
        [SetUp]
        protected override void Init()
        {
            base.Init();
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_SearchWordIsNull()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<ItemNameDto>());

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync(null, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting items names."),
                () => result.Data.Should().BeNull()
            );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_UserIdIsNull()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<ItemNameDto>());

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync("B", null);

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting items names."),
                () => result.Data.Should().BeNull()
            );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_UserIdIsEmpty()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<ItemNameDto>());

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync("B", string.Empty);

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting items names."),
                () => result.Data.Should().BeNull()
            );
        }

        [Test]
        public async Task Should_ReturnSuccessResponseWithAllItemsNamesOfThisUser_When_SearchWordIsEmptyString()
        {
            //Arrange
            var firstFilteredItemNameDto = BuildFirstItemNameDto();
            var secondFilteredItemNameDto = BuildSecondItemNameDto();
            var thirdFilteredItemNameDto = BuildThirdItemNameDto();
            var filteredItemNameDtosAsList = new List<ItemNameDto>() { firstFilteredItemNameDto, secondFilteredItemNameDto, thirdFilteredItemNameDto };
            
            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync(string.Empty, "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .ReturnsAsync(filteredItemNameDtosAsList);

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync(string.Empty, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.Data.Count().Should().Be(3),
                () => result.IsSuccess.Should().BeTrue(),
                () => result.ErrorMessage.Should().BeNull());
        }

        [Test]
        public async Task Should_ReturnResponseWithCorrectItemsNamesOfAllItemsOfThisUser_When_SearchWordIsEmptyString()
        {
            //Arrange
            var firstFilteredItemNameDto = BuildFirstItemNameDto();
            var secondFilteredItemNameDto = BuildSecondItemNameDto();
            var thirdFilteredItemNameDto = BuildThirdItemNameDto();
            var filteredItemNameDtosAsList = new List<ItemNameDto>() { firstFilteredItemNameDto, secondFilteredItemNameDto, thirdFilteredItemNameDto };

            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync(string.Empty, "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .ReturnsAsync(filteredItemNameDtosAsList);

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync(string.Empty, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.Data.First().Name.Should().Be("Bread"),
                () => result.Data.Skip(1).First().Name.Should().Be("Cheese"),
                () => result.Data.Skip(2).First().Name.Should().Be("Biscuits")
            );
        }

        [Test]
        public async Task Should_ReturnSuccessResponseWithOneEntry_When_OneItemNameOfThisUserContainsSearchWord()
        {
            //Arrange
            var filteredItemNameDto = BuildFirstItemNameDto();
            List<ItemNameDto> filteredItemNameDtosAsList = new List<ItemNameDto>() { filteredItemNameDto };

            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync("d", "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .ReturnsAsync(filteredItemNameDtosAsList);

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync("d", "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.Data.Count().Should().Be(1),
                () => result.IsSuccess.Should().BeTrue(),
                () => result.ErrorMessage.Should().BeNull()
                );
        }

        [Test]
        public async Task Should_ReturnResponseWithCorrectItemName_When_OneItemNameOfThisUserContainSearchWord()
        {
            //Arrange
            var filteredItemNameDto = BuildFirstItemNameDto();
            List<ItemNameDto> filteredItemNameDtosAsList = new List<ItemNameDto>() { filteredItemNameDto };

            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync("d", "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .ReturnsAsync(filteredItemNameDtosAsList);

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync("d", "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            result.Data.First().Name.Should().Be("Bread");
        }

        [Test]
        public async Task Should_ReturnSuccessResponseWithTwoEntries_When_TwoItemsNamesOfThisUserContainSearchWord()
        {
            //Arrange
            var firstFilteredItemNameDto = BuildFirstItemNameDto();
            var secondFilteredItemNameDto = BuildThirdItemNameDto();
            var filteredItemNameDtosAsList = new List<ItemNameDto>() { firstFilteredItemNameDto, secondFilteredItemNameDto };

            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync("d", "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .ReturnsAsync(filteredItemNameDtosAsList);

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync("d", "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.Data.Count().Should().Be(2),
                () => result.IsSuccess.Should().BeTrue(),
                () => result.ErrorMessage.Should().BeNull()
                );
        }

        [Test]
        public async Task Should_ReturnResponseWithCorrectItemsNames_When_TwoItemsNamesOfThisUserContainSearchWord()
        {
            //Arrange
            var firstFilteredItemNameDto = BuildFirstItemNameDto();
            var secondFilteredItemNameDto = BuildThirdItemNameDto();
            var filteredItemNameDtosAsList = new List<ItemNameDto>() { firstFilteredItemNameDto, secondFilteredItemNameDto };

            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync("d", "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .ReturnsAsync(filteredItemNameDtosAsList);

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync("d", "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
            () => result.Data.First().Name.Should().Be("Bread"),
            () => result.Data.Skip(1).First().Name.Should().Be("Biscuits"));
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_AnExceptionOccursInRepositoryAllMethod()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemsNamesDtosAsync("B", "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .Throws(new Exception());

            //Act
            var result = await ItemsDataService.GetItemsNamesAsync("B", "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting items names."),
                () => result.Data.Should().BeNull()
            );
        }
    }
}
