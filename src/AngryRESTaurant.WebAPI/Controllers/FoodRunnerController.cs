using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngryRESTaurant.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodRunnerController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }


    }
}
