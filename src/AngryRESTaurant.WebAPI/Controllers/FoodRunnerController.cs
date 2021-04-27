using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngryRESTaurant.WebAPI.Commands;
using MediatR;

namespace AngryRESTaurant.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodRunnerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FoodRunnerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string courierName, string courierService, IEnumerable<Guid> foodId)
        {
            try
            {
                // TODO: Validation
                var response = await _mediator.Send(new CourierOrderRequest(courierName, courierService, foodId));

                return Ok(response);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }

        }
    }
}
