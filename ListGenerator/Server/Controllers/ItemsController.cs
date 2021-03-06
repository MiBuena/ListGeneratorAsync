using IdentityServer4.Extensions;
using ListGenerator.Shared.Dtos;
using ListGenerator.Server.Extensions;
using ListGenerator.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;
using ListGenerator.Data.DB.Migrations;
using System.Threading.Tasks;

namespace ListGenerator.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : IdentityController
    {
        private readonly IItemsDataService _itemsDataService;


        public ItemsController(IItemsDataService itemsDataService)
        {
            _itemsDataService = itemsDataService;
        }

        [HttpGet("itemsnames/{searchWord}")]
        public async Task<IActionResult> GetItemsNamesAsync(string searchWord)
        {
            var response = await _itemsDataService.GetItemsNamesAsync(searchWord, this.UserId);

            if(!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("overview/{dto.PageSize:int?}/{dto.SkipItems:int?}/{dto.OrderByColumn?}/{dto.OrderByDirection?}/{dto.SearchWord?}/{dto.SearchDate?}")]
        public async Task<IActionResult> GetOverviewItems([FromQuery] FilterPatemetersDto dto)
        {
            var response = await _itemsDataService.GetItemsOverviewPageModelAsync(this.UserId, dto);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var response = await _itemsDataService.GetItemAsync(id, UserId);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ItemDto itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _itemsDataService.AddItemAsync(this.UserId, itemDto);

            if(!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Created("items", response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateItem([FromBody] ItemDto itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateResponse = await _itemsDataService.UpdateItemAsync(this.UserId, itemDto);
            if(!updateResponse.IsSuccess)
            {
                return BadRequest(updateResponse);
            }

            return Ok(updateResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var getItemResponse = await _itemsDataService.GetItemAsync(id, UserId);
            if (!getItemResponse.IsSuccess || getItemResponse.Data == null)
            {
                return BadRequest(getItemResponse);
            }

            var deleteResponse = await _itemsDataService.DeleteItemAsync(id, UserId);

            if(!deleteResponse.IsSuccess)
            {
                return BadRequest(deleteResponse);
            }

            return Ok(deleteResponse);
        }
    }
}
