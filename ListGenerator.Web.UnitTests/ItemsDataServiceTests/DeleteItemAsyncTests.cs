using FluentAssertions;
using ListGenerator.Data.Entities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ListGenerator.Web.UnitTests.ItemsDataServiceTests
{
    [TestFixture]
    public class DeleteItemAsyncTests : BaseItemsDataServiceTests
    {
        [SetUp]
        protected override void Init()
        {
            base.Init();
        }

        [Test]
        public async Task Should_ReturnSuccessResponse_When_InputParametersAreValid()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            ItemsRepositoryMock.Setup(c => c.Delete(It.IsAny<Item>()));
            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var response = await ItemsDataService.DeleteItemAsync(1, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");


            //Assert
            AssertHelper.AssertAll(
                () => response.IsSuccess.Should().BeTrue(),
                () => response.ErrorMessage.Should().BeNull()
                );
        }

        [Test]
        public async Task Should_CallSaveMethodAfterDeletemethod_When_InputParametersAreValid()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            var sequence = new MockSequence();
            ItemsRepositoryMock.InSequence(sequence).Setup(x => x.Delete(It.IsAny<Item>()));
            UnitOfWorkMock.InSequence(sequence).Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var response = await ItemsDataService.DeleteItemAsync(1, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");


            //Assert
            AssertHelper.AssertAll(
                () => response.IsSuccess.Should().BeTrue(),
                () => response.ErrorMessage.Should().BeNull()
                );
        }

        [Test]
        public async Task Should_CallDeleteMethodWithCorrectItem_When_InputParametersAreValid()
        {
            //Arrange
            var firstItem = BuildFirstItem();
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            var deleteObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Delete(It.IsAny<Item>()))
                        .Callback<Item>((obj) => deleteObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var response = await ItemsDataService.DeleteItemAsync(1, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");


            //Assert
            AssertHelper.AssertAll(
                () => deleteObject.Id.Should().Be(1),
                () => deleteObject.Name.Should().Be("Bread"),
                () => deleteObject.NextReplenishmentDate.Should().BeSameDateAs(new DateTime(2020, 10, 06)),
                () => deleteObject.ReplenishmentPeriod.Should().Be(1),
                () => deleteObject.UserId.Should().Be("ab70793b-cec8-4eba-99f3-cbad0b1649d0")
                );
        }

        [Test]
        public async Task Should_ReturnErrorResponse_When_CurrentUserDoesNotHaveItemWithThisId()
        {
            //Arrange
            Item firstItem = null;
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            var deleteObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Delete(It.IsAny<Item>()))
                        .Callback<Item>((obj) => deleteObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var response = await ItemsDataService.DeleteItemAsync(5, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");


            //Assert
            AssertHelper.AssertAll(
                () => response.IsSuccess.Should().BeFalse(),
                () => response.ErrorMessage.Should().Be("Current user does not have item with id 5")
                );
        }

        [Test]
        public async Task Should_NotDeleteAnyItem_When_CurrentUserDoesNotHaveItemWithThisId()
        {
            //Arrange
            Item firstItem = null;
            ItemsRepositoryMock
                .Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Item, bool>>>(), null, default))
                .ReturnsAsync(firstItem);

            var deleteObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Delete(It.IsAny<Item>()))
                        .Callback<Item>((obj) => deleteObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var response = await ItemsDataService.DeleteItemAsync(5, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");


            //Assert
            AssertHelper.AssertAll(
                  () => ItemsRepositoryMock.Verify(x => x.Delete(It.IsAny<Item>()), Times.Never()),
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

            var deleteObject = new Item();
            ItemsRepositoryMock.Setup(c => c.Delete(It.IsAny<Item>()))
                        .Callback<Item>((obj) => deleteObject = obj);

            UnitOfWorkMock.Setup(c => c.SaveChangesAsync()).ReturnsAsync(1);


            //Act
            var response = await ItemsDataService.DeleteItemAsync(1, string.Empty);


            //Assert
            AssertHelper.AssertAll(
                () => response.IsSuccess.Should().BeFalse(),
                () => response.ErrorMessage.Should().Be("An error occurred while deleting item")
                );
        }


        //        [Test]
        //        public void Should_NotDeleteAnyItem_When_UserIdIsEmpty()
        //        {
        //            //Arrange
        //            var allItems = BuildItemsCollection();
        //            ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //            var deleteObject = new Item();
        //            ItemsRepositoryMock.Setup(c => c.Delete(It.IsAny<Item>()))
        //                        .Callback<Item>((obj) => deleteObject = obj);

        //            ItemsRepositoryMock.Setup(x => x.SaveChanges());


        //            //Act
        //            var response = ItemsDataService.DeleteItemAsync(1, string.Empty);


        //            //Assert
        //            AssertHelper.AssertAll(
        //                  () => ItemsRepositoryMock.Verify(x => x.Delete(It.IsAny<Item>()), Times.Never()),
        //                  () => ItemsRepositoryMock.Verify(x => x.SaveChanges(), Times.Never())
        //                  );
        //        }

        //        [Test]
        //        public void Should_ReturnErrorResponse_When_UserIdIsNull()
        //        {
        //            //Arrange
        //            var allItems = BuildItemsCollection();
        //            ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //            var deleteObject = new Item();
        //            ItemsRepositoryMock.Setup(c => c.Delete(It.IsAny<Item>()))
        //                        .Callback<Item>((obj) => deleteObject = obj);

        //            ItemsRepositoryMock.Setup(x => x.SaveChanges());


        //            //Act
        //            var response = ItemsDataService.DeleteItemAsync(1, null);


        //            //Assert
        //            AssertHelper.AssertAll(
        //                () => response.IsSuccess.Should().BeFalse(),
        //                () => response.ErrorMessage.Should().Be("An error occurred while deleting item")
        //                );
        //        }

        //        [Test]
        //        public void Should_NotDeleteAnyItem_When_UserIdIsNull()
        //        {
        //            //Arrange
        //            var allItems = BuildItemsCollection();
        //            ItemsRepositoryMock.Setup(x => x.All()).Returns(allItems);

        //            var deleteObject = new Item();
        //            ItemsRepositoryMock.Setup(c => c.Delete(It.IsAny<Item>()))
        //                        .Callback<Item>((obj) => deleteObject = obj);

        //            ItemsRepositoryMock.Setup(x => x.SaveChanges());


        //            //Act
        //            var response = ItemsDataService.DeleteItemAsync(1, null);


        //            //Assert
        //            AssertHelper.AssertAll(
        //                  () => ItemsRepositoryMock.Verify(x => x.Delete(It.IsAny<Item>()), Times.Never()),
        //                  () => ItemsRepositoryMock.Verify(x => x.SaveChanges(), Times.Never())
        //                  );
        //        }

    }
}
