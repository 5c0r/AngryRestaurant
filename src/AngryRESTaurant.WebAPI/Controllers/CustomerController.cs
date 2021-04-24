using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngryRESTaurant.WebAPI.Services;

namespace AngryRESTaurant.WebAPI.Controllers
{
    // What does the customer do , he/she comes to the restaurant , and order food
    [ApiController]
    [Route("[controller]")]
    public sealed class CustomerController : ControllerBase
    {
        private readonly OrderingService _orderingService;

        public CustomerController(OrderingService orderingService)
        {
            _orderingService = orderingService;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCustomer(Guid customerId)
        {
            return Ok(customerId);
        }

        [HttpPost("{customerName}/{foodName}")]
        public async Task<IActionResult> Post(string customerName,string foodName)
        {
            try
            {
                var response = await _orderingService.CustomerCallsOrderAsync(customerName, foodName);

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
