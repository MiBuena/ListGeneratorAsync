using AutoMapper;
using ListGeneration.Data.Interfaces;
using ListGenerator.Data.Entities;
using ListGenerator.Shared.Dtos;
using ListGeneratorListGenerator.Data.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListGeneration.Data.Repositories
{
    public class ItemsRepository : AsyncRepository<Item>, IItemsRepository
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
    }
}
