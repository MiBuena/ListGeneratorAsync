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
    public class GetItemsNamesAsyncTests : BaseItemsDataServiceTests
    {
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
    }
}
