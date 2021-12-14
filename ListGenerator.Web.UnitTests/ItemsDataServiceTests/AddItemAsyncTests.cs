using FluentAssertions;
using ListGenerator.Data.Entities;
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
    public class AddItemAsyncTests : BaseItemsDataServiceTests
    {
        [SetUp]
        protected override void Init()
        {
            base.Init();
        }

        [Test]
        public async Task Should_AddItem_When_InputParametersAreValid()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);

            var saveObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()))
                    .Callback<Item>((obj) => saveObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.AddItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", itemDto);

            //Assert
            AssertHelper.AssertAll(
                () => saveObject.Name.Should().Be("Bread"),
                () => saveObject.NextReplenishmentDate.Should().BeSameDateAs(new DateTime(2020, 10, 06)),
                () => saveObject.ReplenishmentPeriod.Should().Be(1)
            );
        }

        [Test]
        public async Task Should_ReturnASuccessResponse_When_InputParametersAreValid()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);

            var saveObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()))
                    .Callback<Item>((obj) => saveObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var result = await ItemsDataService.AddItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", itemDto);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeTrue(),
                () => result.ErrorMessage.Should().BeNull()
                );
        }

        [Test]
        public async Task Should_AddItemOnce_When_InputParametersAreValidAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);

            var saveObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()))
                    .Callback<Item>((obj) => saveObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var result = await ItemsDataService.AddItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", itemDto);


            //Assert
            ItemsRepositoryMock.Verify(c => c.Add(item), Times.Once());
        }

        [Test]
        public async Task Should_FirstAddItemThenCallSaveChanges_When_InputParametersAreValidAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);

            var sequence = new MockSequence();
            ItemsRepositoryMock.InSequence(sequence).Setup(c => c.Add(It.IsAny<Item>()));
            UnitOfWorkMock.InSequence(sequence).Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.AddItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", itemDto);

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeTrue(),
                () => result.ErrorMessage.Should().BeNull()
                );
        }

        [Test]
        public async Task Should_ReturnAnErrorResponse_When_UserIdIsNullAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);

            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()));

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.AddItemAsync(null, itemDto);

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occurred while creating item")
                );
        }

        [Test]
        public async Task Should_CheckForUserIdNullBeforeOtherMethodCalls_When_UserIdIsNullAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);

            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()));

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.AddItemAsync(null, itemDto);


            //Assert
            AssertHelper.AssertAll(
                  () => MapperMock.Verify(x => x.Map<ItemDto, Item>(It.IsAny<ItemDto>()), Times.Never()),
                  () => ItemsRepositoryMock.Verify(x => x.Add(It.IsAny<Item>()), Times.Never()),
                  () => UnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never())
                  );
        }

        [Test]
        public async Task Should_ReturnAnErrorResponse_When_UserIdIsEmptyStringAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);
            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.AddItemAsync(string.Empty, itemDto);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occurred while creating item")
                );
        }

        [Test]
        public async Task Should_CheckForUserIdEmptyBeforeOtherMethodCalls_When_UserIdIsEmptyStringAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);
            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.AddItemAsync(string.Empty, itemDto);


            //Assert
            AssertHelper.AssertAll(
                  () => MapperMock.Verify(x => x.Map<ItemDto, Item>(It.IsAny<ItemDto>()), Times.Never()),
                  () => ItemsRepositoryMock.Verify(x => x.Add(It.IsAny<Item>()), Times.Never()),
                  () => UnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never())
                  );
        }

        [Test]
        public async Task Should_ReturnAnErrorResponse_When_ItemDtoIsNullAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);
            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var result = await ItemsDataService.AddItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", null);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occurred while creating item")
                );
        }


        [Test]
        public async Task Should_ReturnAnErrorResponse_When_MapperThrowsAnExceptionAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Throws(new Exception());

            var saveObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()))
                    .Callback<Item>((obj) => saveObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var result = await ItemsDataService.AddItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", itemDto);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occurred while creating item")
                );
        }

        [Test]
        public async Task Should_ReturnAnErrorResponse_When_UnitOfWorkThrowsAnExceptionAsync()
        {
            //Arrange
            var itemDto = BuildFirstItemDtoWithoutId();
            var item = BuildFirstItemWithoutId();

            MapperMock.Setup(c => c.Map<ItemDto, Item>(itemDto))
                .Returns(item);

            var saveObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Add(It.IsAny<Item>()))
                    .Callback<Item>((obj) => saveObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync())
               .Throws(new Exception());


            //Act
            var result = await ItemsDataService.AddItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", itemDto);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occurred while creating item")
                );
        }
    }
}
