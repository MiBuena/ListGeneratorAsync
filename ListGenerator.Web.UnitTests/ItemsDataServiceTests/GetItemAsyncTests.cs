using FluentAssertions;
using ListGenerator.Shared.Dtos;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ListGenerator.Web.UnitTests.ItemsDataServiceTests
{
    [TestFixture]
    public class GetItemAsyncTests : BaseItemsDataServiceTests
    {
        [SetUp]
        protected override void Init()
        {
            base.Init();
        }

        [Test]
        public async Task Should_ReturnSuccessResponseWithItem_When_CurrentUserHasItemWithThisId()
        {
            //Arrange
            var filteredItemDto = BuildSecondItemDto();
            var filteredItemDtosAsList = new List<ItemDto>() { filteredItemDto };
            ItemsRepositoryMock
                .Setup(c => c.GetItemDtoAsync(2, "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .ReturnsAsync(filteredItemDto);

            //Act
            var result = await ItemsDataService.GetItemAsync(2, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.Data.Should().NotBeNull(),
                () => result.IsSuccess.Should().BeTrue(),
                () => result.ErrorMessage.Should().BeNull()
                );
        }

        [Test]
        public async Task Should_ReturnResponseWithCorrectItem_When_CurrentUserHasItemWithThisId()
        {
            //Arrange
            var filteredItemDto = BuildSecondItemDto();
            var filteredItemDtosAsList = new List<ItemDto>() { filteredItemDto };
            ItemsRepositoryMock
                .Setup(c => c.GetItemDtoAsync(2, "ab70793b-cec8-4eba-99f3-cbad0b1649d0"))
                .ReturnsAsync(filteredItemDto);

            //Act
            var result = await ItemsDataService.GetItemAsync(2, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.Data.Id.Should().Be(2),
                () => result.Data.Name.Should().Be("Cheese"),
                () => result.Data.NextReplenishmentDate.Should().BeSameDateAs(new DateTime(2020, 10, 08)),
                () => result.Data.ReplenishmentPeriod.Should().Be(2)
                );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_UserIdIsNull()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemDtoAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new ItemDto());

            //Act
            var result = await ItemsDataService.GetItemAsync(1, null);

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting item"),
                () => result.Data.Should().BeNull()
            );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_UserIdIsEmpty()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemDtoAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new ItemDto());

            //Act
            var result = await ItemsDataService.GetItemAsync(1, string.Empty);

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting item"),
                () => result.Data.Should().BeNull()
            );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_ItemIdIsZero()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemDtoAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new ItemDto());

            //Act
            var result = await ItemsDataService.GetItemAsync(0, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting item"),
                () => result.Data.Should().BeNull()
            );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_ItemIdIsNegative()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemDtoAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new ItemDto());

            //Act
            var result = await ItemsDataService.GetItemAsync(-5, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting item"),
                () => result.Data.Should().BeNull()
            );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_CurrentUserDoesNotHaveItemWithThisId()
        {
            //Arrange
            ItemDto itemDtoNull = null;
            ItemsRepositoryMock
                .Setup(c => c.GetItemDtoAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(itemDtoNull);

            //Act
            var result = await ItemsDataService.GetItemAsync(5, "925912b0-c59c-4e1b-971a-06e8abab7848");

            //Assert
            AssertHelper.AssertAll(
                () => result.Data.Should().BeNull(),
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("Current user does not have item with id 5")
                );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_ItemsRepositoryMockThrowsAnException()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(c => c.GetItemDtoAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            //Act
            var result = await ItemsDataService.GetItemAsync(5, "925912b0-c59c-4e1b-971a-06e8abab7848");

            //Assert
            AssertHelper.AssertAll(
                () => result.Data.Should().BeNull(),
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occured while getting item")
                );
        }
    }
}
