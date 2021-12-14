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
        public async Task Should_UpdateItem_When_InputParametersAreValid()
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
        public async Task Should_ReturnSuccessResponse_When_InputParametersAreValid()
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
        public async Task Should_CallRepositorySaveChangesAfterUpdateMethod_When_InputParametersAreValid()
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

        [Test]
        public async Task Should_ReturnErrorResponse_When_UserIdIsNull()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };

            //Act
            var result = await ItemsDataService.UpdateItemAsync(null, updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occurred while updating item")
                );
        }

        [Test]
        public async Task Should_CheckForUserIdNullBeforeAllOtherMethodCalls_When_UserIdIsNull()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), It.IsAny<Func<IQueryable<Item>, IOrderedQueryable<Item>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };

            //Act
            var result = await ItemsDataService.UpdateItemAsync(null, updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                () => ItemsRepositoryMock.Verify(x => 
                x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), 
                It.IsAny<Func<IQueryable<Item>, IOrderedQueryable<Item>>>(), It.IsAny<CancellationToken>()), Times.Never()),
                () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
                () => UnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never())
                );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_UserIdIsEmpty()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };


            //Act
            var result = await ItemsDataService.UpdateItemAsync(string.Empty, updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occurred while updating item")
                );
        }

        [Test]
        public async Task Should_CheckForUserIdEmptyBeforeAllOtherMethodCalls_When_UserIdIsEmpty()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };


            //Act
            var result = await ItemsDataService.UpdateItemAsync(string.Empty, updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                () => ItemsRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(),
                It.IsAny<Func<IQueryable<Item>, IOrderedQueryable<Item>>>(), It.IsAny<CancellationToken>()), Times.Never()),
                () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
                () => UnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never())
                );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_ItemDtoIsNull()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", null);


            //Assert
            AssertHelper.AssertAll(
                () => result.IsSuccess.Should().BeFalse(),
                () => result.ErrorMessage.Should().Be("An error occurred while updating item")
                );
        }

        [Test]
        public async Task Should_CheckForItemDtoNullBeforeAllOtherMethodCalls_When_ItemDtoIsNull()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", null);


            //Assert
            AssertHelper.AssertAll(
                () => ItemsRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(),
                It.IsAny<Func<IQueryable<Item>, IOrderedQueryable<Item>>>(), It.IsAny<CancellationToken>()), Times.Never()),
                () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
                () => UnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never())
                );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_ItemWithThisIdDoesNotExist()
        {
            //Arrange
            Item itemNull = null;
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(itemNull);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var updatedItemDto = new ItemDto()
            {
                Id = 10,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };


            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                 () => result.IsSuccess.Should().BeFalse(),
                 () => result.ErrorMessage.Should().Be("Current user does not have item with id 10")
                 );
        }

        [Test]
        public async Task Should_NotUpdateAnyItemInTheDb_When_ItemWithThisIdDoesNotExist()
        {
            //Arrange
            Item itemNull = null;
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(itemNull);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var updatedItemDto = new ItemDto()
            {
                Id = 10,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };


            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                () => ItemsRepositoryMock.Verify(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(),
                It.IsAny<Func<IQueryable<Item>, IOrderedQueryable<Item>>>(), It.IsAny<CancellationToken>()), Times.Once()),
                () => ItemsRepositoryMock.Verify(x => x.Update(It.IsAny<Item>()), Times.Never()),
                () => UnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never())
                );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_RepositoryFirstOrDefaultAsyncThrowsAnException()
        {
            //Arrange
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .Throws(new Exception());

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };


            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                 () => result.IsSuccess.Should().BeFalse(),
                 () => result.ErrorMessage.Should().Be("An error occurred while updating item")
                 );
        }


        [Test]
        public async Task Should_ReturnErrorResponse_When_RepositoryUpdateThrowsAnException()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>())).Throws(new Exception());
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);

            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };


            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                 () => result.IsSuccess.Should().BeFalse(),
                 () => result.ErrorMessage.Should().Be("An error occurred while updating item")
                 );
        }


        [Test]
        public async Task Should_ReturnErrorResponse_When_RepositorySaveChangesThrowsAnException()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Update(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).Throws(new Exception());

            var updatedItemDto = new ItemDto()
            {
                Id = 1,
                Name = "Bread updated",
                NextReplenishmentDate = new DateTime(2020, 10, 10),
                ReplenishmentPeriod = 4
            };


            //Act
            var result = await ItemsDataService.UpdateItemAsync("ab70793b-cec8-4eba-99f3-cbad0b1649d0", updatedItemDto);


            //Assert
            AssertHelper.AssertAll(
                 () => result.IsSuccess.Should().BeFalse(),
                 () => result.ErrorMessage.Should().Be("An error occurred while updating item")
                 );
        }
    }
}
