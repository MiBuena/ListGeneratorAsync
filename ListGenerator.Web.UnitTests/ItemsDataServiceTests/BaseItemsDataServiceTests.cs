using AutoMapper;
using ListGeneration.Data.Interfaces;
using ListGenerator.Server.CommonResources;
using ListGenerator.Server.Interfaces;
using ListGenerator.Server.Services;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListGenerator.Web.UnitTests.ItemsDataServiceTests
{
    public class BaseItemsDataServiceTests
    {
        [SetUp]
        protected virtual void Init()
        {
            ItemsRepositoryMock = new Mock<IItemsRepository>(MockBehavior.Strict);
            UnitOfWorkMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            UnitOfWorkMock.Setup(x => x.ItemsRepository).Returns(ItemsRepositoryMock.Object);

            MapperMock = new Mock<IMapper>(MockBehavior.Strict);
            StringLocalizerMock = new Mock<IStringLocalizer<Errors>>(MockBehavior.Strict);
            ItemsDataService = new ItemsDataService(UnitOfWorkMock.Object, MapperMock.Object, StringLocalizerMock.Object);

        }

        protected Mock<IItemsRepository> ItemsRepositoryMock { get; private set; }
        protected Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }
        protected Mock<IMapper> MapperMock { get; private set; }
        protected Mock<IStringLocalizer<Errors>> StringLocalizerMock { get; private set; }
        protected IItemsDataService ItemsDataService { get; private set; }

    }
}
