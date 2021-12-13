﻿using ListGenerator.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ListGeneration.Data.Interfaces
{
    public interface IItemsRepository
    {
        Task<IEnumerable<ItemNameDto>> GetItemsNamesDtosAsync(string searchWord, string userId);
    }
}
