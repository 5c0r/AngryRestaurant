using AngryRESTaurant.WebAPI.Model;
using AngryRESTaurant.WebAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AngryRESTaurant.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IRepository<FoodMenu> _menuRepository;

        public MenuController(IRepository<FoodMenu> menuRepository)
        {
            this._menuRepository = menuRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var allMenu = await this._menuRepository.QueryAllAsync();

                return Ok(allMenu.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                // TODO: Check if return null
                var menuItem = (await this._menuRepository.SearchAsync(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))).SingleOrDefault();

                if (menuItem == null)
                    return NotFound();

                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]FoodMenuInputModel newFood)
        {
            try
            {
                var newFoodToAdd = new FoodMenu
                {
                    Id = Guid.NewGuid(),
                    Name = newFood.Name,
                    CookingTime = newFood.CookingTime,
                    IsDifficult = newFood.IsDifficult
                };

                this._menuRepository.Upsert(newFoodToAdd);

                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]FoodMenuInputModel foodPayload)
        {
            try
            {
                var foodItemToUpdate = this._menuRepository.GetById(id);

                if (foodItemToUpdate == null)
                    return NotFound();

                foodItemToUpdate.CookingTime = foodPayload.CookingTime;
                foodItemToUpdate.Name = foodPayload.Name;
                foodItemToUpdate.IsDifficult = foodPayload.IsDifficult;

                this._menuRepository.Upsert(foodItemToUpdate);

                return Accepted();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await this._menuRepository.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
