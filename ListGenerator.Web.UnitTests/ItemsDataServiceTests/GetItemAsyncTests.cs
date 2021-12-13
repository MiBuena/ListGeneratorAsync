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
    }
}
