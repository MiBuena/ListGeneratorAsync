using FluentAssertions;
using ListGenerator.Data.Entities;
using ListGenerator.Shared.Dtos;
using ListGenerator.Web.UnitTests.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ListGenerator.Web.UnitTests.ItemsDataServiceTests
{
    [TestFixture]
    public class UpdateItemAsyncTests : BaseItemsDataServiceTests
    {
        [SetUp]
        protected override void Init()
        {
            base.Init();
        }

        [Test]
        public async Task Should_UpdateItem_When_InputParametersAreValidAsync()
        {
            //Arrange
            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };

            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            var saveObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Update(It.Is<Item>(x =>
                x.HasTheSameProperties("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto))))
                    .Callback<Item>((obj) => saveObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                () => saveObject.Id.Should().Be(1),
                () => saveObject.Name.Should().Be("Bread updated"),
                () => saveObject.NextReplenishmentDate.Should().BeSameDateAs(new DateTime(2020, 10, 10)),
                () => saveObject.ReplenishmentPeriod.Should().Be(4)
                );
        }

        [Test]
        public async Task Should_ReturnSuccessResponse_When_InputParametersAreValidAsync()
        {
            //Arrange
            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };

            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            var saveObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Update(It.Is<Item>(x =>
                x.HasTheSameProperties("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto))))
                    .Callback<Item>((obj) => saveObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeTrue(),
                () => result.ErrorMessage.Should().BeNull()
                );
        }

        [Test]
        public async Task Should_CallRepositorySaveChangesAfterUpdateMethod_When_InputParametersAreValidAsync()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };

            var sequence = new MockSequence();

            ItemsRepositoryMock
                .InSequence(sequence)
                .Setup(c => c.Update(It.Is<Item>(x =>
                x.HasTheSameProperties("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto))));

            UnitOfWorkMock.InSequence(sequence).Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);

            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeTrue(),
                () => result.ErrorMessage.Should().BeNull()
                );
        }

        //[Test]
        //public void Should_ReturnErrorResponse_When_UserIdIsNull()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 1,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };

        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync(null, updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //        () => result.IsSuccess.Should().BeFalse(),
        //        () => result.ErrorMessage.Should().Be("An error occurred while updating item")
        //        );
        //}

        //[Test]
        //public void Should_CheckForUserIdNullBeforeAllOtherMethodCalls_When_UserIdIsNull()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 1,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };

        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync(null, updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //        () => ItemsRepositoryMock.Verify(x => x.All(), Times.Never()),
        //        () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
        //        () => ItemsRepositoryMock.Verify(x => x.SaveChanges(), Times.Never())
        //        );
        //}

        //[Test]
        //public void Should_ReturnErrorResponse_When_UserIdIsEmpty()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 1,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };


        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync(string.Empty, updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //        () => result.IsSuccess.Should().BeFalse(),
        //        () => result.ErrorMessage.Should().Be("An error occurred while updating item")
        //        );
        //}

        //[Test]
        //public void Should_CheckForUserIdEmptyBeforeAllOtherMethodCalls_When_UserIdIsEmpty()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 1,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };

        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync(string.Empty, updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //        () => ItemsRepositoryMock.Verify(x => x.All(), Times.Never()),
        //        () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
        //        () => ItemsRepositoryMock.Verify(x => x.SaveChanges(), Times.Never())
        //        );
        //}

        //[Test]
        //public void Should_ReturnErrorResponse_When_ItemDtoIsNull()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());


        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", null);


        //    //Assert
        //    AssertHelper.AssertAll(
        //        () => result.IsSuccess.Should().BeFalse(),
        //        () => result.ErrorMessage.Should().Be("An error occurred while updating item")
        //        );
        //}

        //[Test]
        //public void Should_CheckForItemDtoNullBeforeAllOtherMethodCalls_When_ItemDtoIsNull()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 1,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };

        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", null);


        //    //Assert
        //    AssertHelper.AssertAll(
        //        () => ItemsRepositoryMock.Verify(x => x.All(), Times.Never()),
        //        () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
        //        () => ItemsRepositoryMock.Verify(x => x.SaveChanges(), Times.Never())
        //        );
        //}

        //[Test]
        //public void Should_ReturnErrorResponse_When_ItemWithThisIdDoesNotExist()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 10,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };


        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //         () => result.IsSuccess.Should().BeFalse(),
        //         () => result.ErrorMessage.Should().Be("Current user does not have item with id 10")
        //         );
        //}

        //[Test]
        //public void Should_NotUpdateAnyItemInTheDb_When_ItemWithThisIdDoesNotExist()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 10,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };

        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //        () => ItemsRepositoryMock.Verify(x => x.All(), Times.Once()),
        //        () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
        //        () => ItemsRepositoryMock.Verify(x => x.SaveChanges(), Times.Never())
        //        );
        //}

        //[Test]
        //public void Should_ReturnErrorResponse_When_AnotherUserHasThisItem()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 5,
        //        Name = "Cake updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };


        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //         () => result.IsSuccess.Should().BeFalse(),
        //         () => result.ErrorMessage.Should().Be("Current user does not have item with id 5")
        //         );
        //}


        //[Test]
        //public void Should_NotUpdateAnyItemInTheDb_When_AnotherUserHasThisItem()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 5,
        //        Name = "Cake updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };


        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //        () => ItemsRepositoryMock.Verify(x => x.All(), Times.Once()),
        //        () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
        //        () => ItemsRepositoryMock.Verify(x => x.SaveChanges(), Times.Never())
        //        );
        //}


        //[Test]
        //public void Should_ReturnErrorResponse_When_RepositoryAllThrowsAnException()
        //{
        //    //Arrange
        //    ItemsRepositoryMock.Setup(x => x.All()).Throws(new Exception());

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 1,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };


        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //         () => result.IsSuccess.Should().BeFalse(),
        //         () => result.ErrorMessage.Should().Be("An error occurred while updating item")
        //         );
        //}


        //[Test]
        //public void Should_ReturnErrorResponse_When_RepositoryUpdateThrowsAnException()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>())).Throws(new Exception());
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 1,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };


        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //         () => result.IsSuccess.Should().BeFalse(),
        //         () => result.ErrorMessage.Should().Be("An error occurred while updating item")
        //         );
        //}


        //[Test]
        //public void Should_ReturnErrorResponse_When_RepositorySaveChangesThrowsAnException()
        //{
        //    //Arrange
        //    var allItems = BuildItemsCollection();
        //    ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //    ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
        //    ItemsRepositoryMock.Setup(c => c.SaveChanges()).Throws(new Exception());

        //    var updatedItemDto = new ItemDto()
        //    {
        //        Id = 1,
        //        Name = "Bread updated",
        //        NextReplenishmentDate = new DateTime(2020, 10, 10),
        //        ReplenishmentPeriod = 4
        //    };


        //    //Act
        //    var result = ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


        //    //Assert
        //    AssertHelper.AssertAll(
        //         () => result.IsSuccess.Should().BeFalse(),
        //         () => result.ErrorMessage.Should().Be("An error occurred while updating item")
        //         );
        //}
    }
}
