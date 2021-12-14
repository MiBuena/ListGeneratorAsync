using AutoMapper;
using ListGeneration.Data.Interfaces;
using ListGenerator.Data.Entities;
using ListGenerator.Server.Interfaces;
using ListGenerator.Server.Services;
using Moq;
using NUnit.Framework;

namespace ListGenerator.Web.UnitTests.ReplenishmentDataServiceTests
{
    public class BaseReplenishmentDataServiceTests : BaseItemsTests
    {
        [SetUp]
        protected virtual void Init()
        {
            ItemsRepositoryMock = new Mock<IItemsRepository>(MockBehavior.Strict);
            PurchaseRepositoryMock = new Mock<IAsyncRepository<Purchase>>(MockBehavior.Strict);
            UnitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            UnitOfWorkMock.Setup(x => x.ItemsRepository).Returns(ItemsRepositoryMock.Object);
            UnitOfWorkMock.Setup(x => x.PurchasesRepository).Returns(PurchaseRepositoryMock.Object);

            MapperMock = new Mock<IMapper>(MockBehavior.Strict);
            ReplenishmentItemBuilderMock = new Mock<IReplenishmentItemBuilder>(MockBehavior.Strict);
            ReplenishmentDataService = new ReplenishmentDataService(UnitOfWorkMock.Object, MapperMock.Object, ReplenishmentItemBuilderMock.Object);
        }

        protected Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }
        protected Mock<IItemsRepository> ItemsRepositoryMock { get; private set; }
        protected Mock<IAsyncRepository<Purchase>> PurchaseRepositoryMock { get; private set; }
        protected Mock<IMapper> MapperMock { get; private set; }
        protected Mock<IReplenishmentItemBuilder> ReplenishmentItemBuilderMock { get; private set; }
        protected IReplenishmentDataService ReplenishmentDataService { get; private set; }
    }
}
