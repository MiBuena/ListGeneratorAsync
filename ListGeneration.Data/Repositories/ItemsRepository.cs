using AutoMapper;
using ListGeneration.Data.Interfaces;
using ListGenerator.Data.Entities;
using ListGenerator.Shared.Extensions;
using ListGenerator.Shared.Dtos;
using ListGenerator.Shared.Enums;
using ListGenerator.Shared.Helpers;
using ListGeneratorListGenerator.Data.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListGeneration.Data.Repositories
{
    public class ItemsRepository : EfRepository<Item>, IItemsRepository
    {
        private readonly IMapper _mapper;

        public ItemsRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemNameDto>> GetItemsNamesDtosAsync(string searchWord, string userId)
        {
            var query = this.Context.Items
                .Where(x => x.UserId == userId);

            if (!string.IsNullOrEmpty(searchWord))
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchWord.ToLower()));
            }

            var names = await _mapper.ProjectTo<ItemNameDto>(query)
                .ToListAsync();

            return names;
        }

        public async Task<ItemDto> GetItemDtoAsync(int itemId, string userId)
        {
            var query = this.Context.Items.Where(x => x.Id == itemId && x.UserId == userId);

            var dto = await _mapper.ProjectTo<ItemDto>(query)
                .FirstOrDefaultAsync();

            return dto;
        }

        public async Task<ItemsOverviewPageDto> GetItemsOverviewPageDtoAsync(string userId, FilterPatemetersDto dto)
        {
            var query = GetOverviewItemsQuery(userId, dto);

            var dtos = await query
                .Skip(dto.SkipItems.Value)
                .Take(dto.PageSize.Value)
                .ToListAsync();

            var itemsCount = await query.CountAsync();

            var pageDto = new ItemsOverviewPageDto()
            {
                OverviewItems = dtos,
                TotalItemsCount = itemsCount
            };

            return pageDto;
        }

        private IQueryable<ItemOverviewDto> GetOverviewItemsQuery(string userId, FilterPatemetersDto dto)
        {
            var query = GetBaseQuery(userId);

            query = FilterBySearchWord(dto.SearchWord, query);

            query = FilterBySearchDate(dto.SearchDate, query);

            query = Sort(dto.OrderByColumn, dto.OrderByDirection, query);

            return query;
        }

        private IQueryable<ItemOverviewDto> FilterBySearchDate(string searchDate, IQueryable<ItemOverviewDto> query)
        {
            if (searchDate != null)
            {
                var parsedDate = DateTimeHelper.ToDateFromTransferDateAsString(searchDate);

                query = query
                    .Where(x => x.LastReplenishmentDate == parsedDate
                || x.NextReplenishmentDate == parsedDate);
            }

            return query;
        }

        private IQueryable<ItemOverviewDto> FilterBySearchWord(string searchWord, IQueryable<ItemOverviewDto> query)
        {
            if (searchWord != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(searchWord.ToLower()));
            }

            return query;
        }

        private IQueryable<ItemOverviewDto> Sort(string orderByColumn, SortingDirection? orderByDirection, IQueryable<ItemOverviewDto> query)
        {
            if (orderByColumn != null && orderByDirection != null)
            {
                if (orderByDirection == SortingDirection.Ascending)
                {
                    query = query.OrderByProperty(orderByColumn);
                }
                else
                {
                    query = query.OrderByPropertyDescending(orderByColumn);
                }
            }

            return query;
        }

        private IQueryable<ItemOverviewDto> GetBaseQuery(string userId)
        {
            var query = this.Context.Items
                .Where(x => x.UserId == userId)
                .Select(x => new ItemOverviewDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ReplenishmentPeriod = x.ReplenishmentPeriod,
                    NextReplenishmentDate = x.NextReplenishmentDate,
                    LastReplenishmentDate = x.Purchases
                                     .OrderByDescending(y => y.ReplenishmentDate)
                                     .Select(m => (DateTime?)m.ReplenishmentDate)
                                     .FirstOrDefault(),
                    LastReplenishmentQuantity = x.Purchases
                                     .OrderByDescending(y => y.ReplenishmentDate)
                                     .Select(m => (int?)m.Quantity)
                                     .FirstOrDefault(),
                });

            return query;
        }

        public async Task<IEnumerable<Item>> GetShoppingListItemsAsync(DateTime date, string userId)
        {
            var query = this.Context.Items
                .Where(x => x.NextReplenishmentDate.Date < date
                && x.UserId == userId)
                .OrderBy(x => x.NextReplenishmentDate);

            var itemsNeedingReplenishment = await query.ToListAsync();

            return itemsNeedingReplenishment;
        }
    }
}
