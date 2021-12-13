using ListGenerator.Shared.Dtos;
using ListGenerator.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ListGenerator.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReplenishmentController : IdentityController
    {
        private readonly IReplenishmentDataService _replenishmentDataService;

        public ReplenishmentController(IReplenishmentDataService replenishmentDataService)
        {
            _replenishmentDataService = replenishmentDataService;
        }


        [HttpGet("shoppinglist/{firstReplenishmentDate}/{secondReplenishmentDate}")]
        public async Task<IActionResult> GetShoppingListAsync(string firstReplenishmentDate, string secondReplenishmentDate)
        {
            var response = await _replenishmentDataService.GetShoppingListAsync(firstReplenishmentDate, secondReplenishmentDate, this.UserId);
           
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("replenish")]
        public IActionResult ReplenishItems([FromBody] ReplenishmentDto model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _replenishmentDataService.ReplenishItemsAsync(model);

            return Ok();
        }
    }
}
