using AutoMapper;
using FluentAssertions;
using IdentityServer4.EntityFramework.Options;
using ListGeneration.Data.Repositories;
using ListGenerator.Data.Entities;
using ListGenerator.Shared.Dtos;
using ListGeneratorListGenerator.Data.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ListGenerator.Web.UnitTests.ItemsRepositoryTests
{
    [TestFixture]
    public class GetItemDtoAsyncTests : BaseItemsTests
    {
        [SetUp]
        protected void Init()
        {
            ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            OperationalStoreOptions = new Mock<IOptions<OperationalStoreOptions>>();
            OperationalStoreOptions.Setup(x => x.Value)
                .Returns(new OperationalStoreOptions()
                {
                    DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
                    EnableTokenCleanup = false,
                    PersistedGrants = new TableConfiguration("PersistedGrants"),
                    TokenCleanupBatchSize = 100,
                    TokenCleanupInterval = 3600
                }
            );

            MapperMock = new Mock<IMapper>();

        }

        public DbContextOptions<ApplicationDbContext> ContextOptions { get; private set; }
        public Mock<IOptions<OperationalStoreOptions>> OperationalStoreOptions { get; private set; }
        public Mock<IMapper> MapperMock { get; private set; }



        [Test]
        public async Task Should_ReturnCorrectItem_When_UserWithThisIdHasItemWithThisId()
        {
            using (var context = new ApplicationDbContext(ContextOptions, OperationalStoreOptions.Object))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var firstItem = BuildFirstItem();
                context.Items.Add(firstItem);
                context.SaveChanges();

                var firstItemDto = BuildFirstItemDto();
                List<ItemDto> itemDtosCollection = new List<ItemDto>() { firstItemDto };
                IQueryable<ItemDto> itemDtosCollectionAsQueryable = itemDtosCollection.AsQueryable();

                MapperMock.Setup(c => c.ProjectTo(
                    It.IsAny<IQueryable<Item>>(),
                    null,
                    It.Is<Expression<Func<ItemDto, object>>[]>(x => x.Length == 0)))
                    .Returns(itemDtosCollectionAsQueryable);

                var itemsRepository = new ItemsRepository(context, MapperMock.Object);

                var result = await itemsRepository.GetItemDtoAsync(1, "ab70793b-cec8-4eba-99f3-cbad0b1649d0");

                //Assert
                AssertHelper.AssertAll(
                    () => result.Id.Should().Be(1)
                    //() => result.IsSuccess.Should().BeTrue(),
                    //() => result.ErrorMessage.Should().BeNull()
                    );
            }
        }
    }
}
