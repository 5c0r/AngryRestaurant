using System;
using System.Threading.Tasks;
using AngryRESTaurant.WebAPI.Model;
using AngryRESTaurant.WebAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AngryRESTaurant.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class OrderController : ControllerBase
    {
        private readonly IRepository<OrderStatus> _orderRepository;

        public OrderController(IRepository<OrderStatus> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allOrders = await _orderRepository.QueryAllAsync();
            return Ok(allOrders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var oneOrder = await _orderRepository.GetByIdAsync(id);

            if (oneOrder == null)
            {
                return NotFound();
            }

            return Ok(oneOrder);
        }
    }
}